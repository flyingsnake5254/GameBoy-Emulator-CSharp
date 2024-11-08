using u8 = System.Byte;
using u16 = System.Int16;


public class CPU
{
    public struct Registers
    {
        public u8 A, F, B, C, D, E, H, L;
        public u16 SP, PC;

        public Registers(
            u8 a = 0x00, u8 f = 0x00, 
            u8 b = 0x00, u8 c = 0x00, 
            u8 d = 0x00, u8 e = 0x00, 
            u8 h = 0x00, u8 l = 0x00,
            u16 sp = 0x0000, u16 pc = 0x0000
        )
        {
            A = a; F = f;
            B = b; C = c;
            D = d; E = e;
            H = h; L = l;
            SP = sp; PC = pc;
        }
    }


    /*
        Error Code
    */
    private const int UNKNOWN_ADDRESS_MODE = -6;
    private const int UNKNOWN_INSTRUCTION = -7;


    /*
        CPU 成員變數
    */
    private Registers _registers;
    private u16 _fetchData;
    private u16 _memoryDestination;
    private u8 _currentOpcode;
    private bool _hallted;
    private bool _stepping;
    private Instruction _currentInstruction;


    // CPU 建構子
    public CPU()
    {
        _registers = new Registers();
    }



    public void Init()
    {
        _registers.PC = 0x100;
        Console.WriteLine("CPU 初始化成功");
    }

    public bool Step()
    {
        if (_hallted == false)
        {
            FetchInstruction();
            FetchData();
            Execute();
        }
        return true;
    }


    public void FetchInstruction()
    {
        _currentOpcode = Bus.BusRead(_registers.PC ++);
        _currentInstruction = Instruction.InstructionByOpcode(_currentOpcode);

        if (_currentInstruction == null)
        {
            Console.WriteLine($"Unknown Instruction : {_currentOpcode, 0:X2}");
            Environment.Exit(UNKNOWN_INSTRUCTION);
        }
    }


    public void FetchData()
    {
        // 初始化狀態
        _memoryDestination = 0x0000;
        bool destinationIsMemory = false;


        // 是否有指令
        if (_currentInstruction == null)
        {
            return;
        }


        // Address Mode 
        switch (_currentInstruction.AddressMode)
        {
            case Instruction.EAddressMode.Implicit:
                return;

            case Instruction.EAddressMode.Reg:
                _fetchData = ReadReg(_currentInstruction.RegisterType1);
                return;

            case Instruction.EAddressMode.RegD8:
                _fetchData = Bus.BusRead(_registers.PC);
                Emulator.EmulatorCycles(1);
                _registers.PC ++;
                return;
            
            case Instruction.EAddressMode.D16:
                u8 low = Bus.BusRead(_registers.PC);
                Emulator.EmulatorCycles(1);

                u8 high = Bus.BusRead(_registers.PC);
                Emulator.EmulatorCycles(1);

                _fetchData = (u16) (low | (high << 8));

                _registers.PC += 2;
                return;

            default:
                Console.WriteLine("Unknown Address Mode");
                Environment.Exit(UNKNOWN_ADDRESS_MODE);
                return;
        }
    }


    public void Execute()
    {
        Console.WriteLine($"Execute() : Current Opcode - {_currentOpcode, 0:X2} | PC : {_registers.PC, 0:X4}");
        // Console.WriteLine("Execute 尚未實現");
    }


    private u16 ReadReg(Instruction.ERegisterType regType)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A:
                return _registers.A;
            
            case Instruction.ERegisterType.F:
                return _registers.F;

            case Instruction.ERegisterType.B:
                return _registers.B;

            case Instruction.ERegisterType.C:
                return _registers.C;

            case Instruction.ERegisterType.D:
                return _registers.D;

            case Instruction.ERegisterType.E:
                return _registers.E;

            case Instruction.ERegisterType.H:
                return _registers.H;

            case Instruction.ERegisterType.L:
                return _registers.L;


            case Instruction.ERegisterType.AF:
                return (u16) ((_registers.F << 8) | _registers.A);

            case Instruction.ERegisterType.BC:
                return (u16) ((_registers.C << 8) | _registers.B);

            case Instruction.ERegisterType.DE:
                return (u16) ((_registers.E << 8) | _registers.D);

            case Instruction.ERegisterType.HL:
                return (u16) ((_registers.L << 8) | _registers.H);

            
            case Instruction.ERegisterType.PC:
                return _registers.PC;

            case Instruction.ERegisterType.SP:
                return _registers.SP;

            default:
                return 0;
        }
    }
}