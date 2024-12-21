using Newtonsoft.Json;

public class CPU
{
    // 暫存器
    [JsonProperty]
    private Registers _regs;

    // 指令集
    [JsonProperty]
    private Instructions _insc;

    // Memory
    [JsonProperty]
    private MMU _mmu;

    public void LoadCPU()
    {
        _insc.SetMMU(ref _mmu);
        _insc.SetRegisters(ref _regs);
    }
    public CPU(ref MMU mmu)
    {
        this._mmu = mmu;
        _regs = new Registers();
        _regs.Init();
        _insc = new Instructions(ref _mmu, ref _regs);
    }

    public int Step()
    {
        return _insc.Step();
    }

    public void SetMMU(ref MMU mmu)
    {
        this._mmu = mmu;
    }

    public void Interrupt(int value)
    {
        _insc.ExecuteInterrupt(value);
    }

    public void UpdateIME()
    {
        _insc.UpdateIME();
    }

}