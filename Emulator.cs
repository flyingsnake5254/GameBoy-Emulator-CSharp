using Gtk;


public class Emulator
{
    private MMU _mmu;
    private CPU _cpu;
    private PPU _ppu;
    private Timer _timer;
    private Cartridge _cartridge;
    private MBC1 mbc;
    private Keyboard _keyboard;
    public bool _running = false;
    private int cycles;
    private Record _saveRecord;
    private bool saving = false;
    private bool fromLoadFile = false;

    private CancellationTokenSource _cancellationTokenSource;


    public Emulator()
    {
    }

    public void DefaultInit(string filePath)
    {
        // 取消現有線程
        StopEmulator();
        _cartridge = new Cartridge(filePath);
        mbc = _cartridge.GetMBC();
        _mmu = new MMU(ref mbc);
        _cpu = new CPU(ref _mmu);
        _ppu = new PPU(ref _mmu);
        _ppu.Init();
        _timer = new Timer(ref _mmu);
        _keyboard = Program.keyboard; // 外部傳入的 Keyboard
        _keyboard.Init(ref _mmu);
        cycles = 0;

        _running = true;
        fromLoadFile = false;
        _cancellationTokenSource = new CancellationTokenSource();
        Task t = Task.Run(() => Run(_cancellationTokenSource.Token));
    }

    public void LoadEmulator(Record record)
    {
        // 取消現有線程
        StopEmulator();
        _cartridge = record.Cartridge;
        mbc = record.Mbc;
        _mmu = record.Mmu;
        _mmu.SetMBC(ref mbc);
        _cpu = record.Cpu;
        _cpu.SetMMU(ref _mmu);
        _cpu.LoadCPU();
        _ppu = record.Ppu;
        _ppu.Init();
        _ppu.SetMMU(ref _mmu);
        _timer = record.Timer;
        _timer.SetMMU(ref _mmu);
        _keyboard = Program.keyboard;
        _keyboard.Init(ref _mmu);

        _running = record.Running;
        cycles = record.Cycles;
        fromLoadFile = true;
        _cancellationTokenSource = new CancellationTokenSource();
        Task t = Task.Run(() => Run(_cancellationTokenSource.Token));
    }

    public void StopEmulator()
{
    if (_cancellationTokenSource != null)
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }
    _running = false;
}


    public async Task Run(CancellationToken cancellationToken)
    {
        if (!fromLoadFile) { cycles = 0; }
            
        int returnCycles = 0;
        while (_running)
        {
            while (cycles < 70224) // 一幀需要的時鐘週期數
            {
                // returnCycles = _cpu.Step2();
                returnCycles = _cpu.Step();
                // if (returnCycles == -100) _running = false;
                cycles += returnCycles;

                // 更新 Timer
                _timer.UpdateDIV(returnCycles);
                _timer.UpdateTIMA(returnCycles);

                // 更新 PPU
                _ppu.Update(returnCycles);

                // 更新鍵盤輸入
                _keyboard.Update();

                // 中斷處理
                u8 IE = _mmu.GetIE();
                u8 IF = _mmu.IFRegister;
                for (int i = 0; i < 5; i++)
                {
                    if ((((IE & IF) >> i) & 0x1) == 1)
                    {
                        _cpu.Interrupt(i);
                    }
                }
                _cpu.UpdateIME();
                if (saving)
                {
                    _saveRecord = new Record(_cpu, _mmu, _ppu, _timer, _cartridge, mbc, cycles, _running);
                    saving = false;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("模擬器線程已取消");
                    break;
                }
            }

            cycles -= 70224;
            if (saving)
            {
                _saveRecord = new Record(_cpu, _mmu, _ppu, _timer, _cartridge, mbc, cycles, _running);
                saving = false;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("模擬器線程已取消");
                break;
            }
        }
    }

    public Record GetGameData()
    {
        Console.WriteLine("start saving");
        saving = true;
        while (saving)
        {

        }
        Console.WriteLine("end saving");
        return new Record(_cpu, _mmu, _ppu, _timer, _cartridge, mbc, cycles, _running);
    }


}
