using u8 = System.Byte;
using u16 = System.UInt16;


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
        Instruction Type 對應到實際指令的對照表
    */
    private Dictionary<Instruction.EInstructionType, Delegate> _processors;

    /*
        Error Code
    */
    private const int UNKNOWN_ADDRESS_MODE = -6;
    private const int UNKNOWN_INSTRUCTION = -7;


    /*
        CPU 成員變數
    */
    public static u8 IERegister { get; set; }
    private Registers _registers;
    private u16 _fetchData;
    private u16 _memoryDestination;
    private u8 _currentOpcode;
    private bool _hallted;
    private bool _stepping;
    private bool _instructionMasterEnable;
    private bool _destinationIsMemory;
    private Instruction _currentInstruction;


    // CPU 建構子
    public CPU()
    {
        _registers = new Registers();
        _processors = new Dictionary<Instruction.EInstructionType, Delegate> 
        {
            { Instruction.EInstructionType.NONE, new Action(ProcNone) },
            { Instruction.EInstructionType.NOP, new Action(ProcNOP) },
            { Instruction.EInstructionType.DI, new Action(ProcDI) },
            { Instruction.EInstructionType.LD, new Action(ProcLD) },
            { Instruction.EInstructionType.XOR, new Action(ProcXOR) },
            { Instruction.EInstructionType.JP, new Action(ProcJP) },
            { Instruction.EInstructionType.LDH, new Action(ProcLDH)},
            { Instruction.EInstructionType.POP, new Action(ProcPOP)},
            { Instruction.EInstructionType.PUSH, new Action(ProcPUSH)},
        };
    }



    public void Init()
    {
        _registers.PC = 0x100;
        Console.WriteLine("V CPU 初始化成功");
    }

    public bool Step()
    {
        if (_hallted == false)
        {
            u16 pc = _registers.PC;
            
            FetchInstruction();
            FetchData();
            Console.WriteLine(
                $"PC: {pc, 0:X4} " +
                $"{Instruction.GetInstructionName(_currentInstruction.InstructionType)} " + 
                $"({_currentOpcode, 0:X2}, {Bus.BusRead((u16) (pc + 1)), 0:X2}, {Bus.BusRead((u16) (pc + 2)), 0:X2}) " +
                $"    AF:{_registers.A, 0:X2}{_registers.F, 0:X2} BC:{_registers.B, 0:X2}{_registers.C, 0:X2} DE:{_registers.D, 0:X2}{_registers.E, 0:X2} HL:{_registers.H, 0:X2}{_registers.L, 0:X2} SP:{_registers.SP, 0:X4}");
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
            Console.WriteLine($"找不到 opcode 對應的 Instruction : {_currentOpcode, 0:X2}");
            Environment.Exit(UNKNOWN_INSTRUCTION);
        }
    }


    public void FetchData()
    {
        // 初始化狀態
        _memoryDestination = 0x0000;
        _destinationIsMemory = false;


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

            case Instruction.EAddressMode.D8:
                _fetchData = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                return;
            
            case Instruction.EAddressMode.RegReg:
                _fetchData = ReadReg(_currentInstruction.RegisterType2);
                return;

            case Instruction.EAddressMode.MemReg:
                _fetchData = ReadReg(_currentInstruction.RegisterType2);
                _memoryDestination = ReadReg(_currentInstruction.RegisterType1);
                _destinationIsMemory = true;

                if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.C)
                {
                    _memoryDestination = (u16) (_memoryDestination | 0xFF00);
                }
                return;
            
            case Instruction.EAddressMode.Reg:
                _fetchData = ReadReg(_currentInstruction.RegisterType1);
                return;

            case Instruction.EAddressMode.A8Reg:
                _memoryDestination = (u16) (Bus.BusRead(_registers.PC ++) | 0xFF00);
                Emulator.EmulatorCycles(1);
                _destinationIsMemory = true;
                return;

            case Instruction.EAddressMode.RegA8:
                _fetchData = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                return;

            case Instruction.EAddressMode.HLSPR:
                _fetchData = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                return;

            case Instruction.EAddressMode.RegA16:
                u8 low3 = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);

                u8 high3 = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);

                u16 addr = (u16) (low3 | (high3 << 8));
                _fetchData = Bus.BusRead(addr);
                Emulator.EmulatorCycles(1);
                return;

            case Instruction.EAddressMode.RegD16:
            case Instruction.EAddressMode.D16:
                u8 low = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);

                u8 high = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);

                _fetchData = (u16) (low | (high << 8));

                return;
            
            case Instruction.EAddressMode.RegMem:
                u16 addr2 = ReadReg(_currentInstruction.RegisterType2);
                if (_currentInstruction.RegisterType2 == Instruction.ERegisterType.C)
                {
                    addr = (u16) (addr2 | 0xFF00);
                }
                _fetchData = Bus.BusRead(addr2);
                Emulator.EmulatorCycles(1);
                return;
            
            case Instruction.EAddressMode.Mem:
                _memoryDestination = ReadReg(_currentInstruction.RegisterType1);
                _destinationIsMemory = true;
                _fetchData = Bus.BusRead(ReadReg(_currentInstruction.RegisterType1));
                Emulator.EmulatorCycles(1);

                return;
            
            case Instruction.EAddressMode.RegD8:
                _fetchData = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                return;
            
            case Instruction.EAddressMode.MemD8:
                _fetchData = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                _memoryDestination = ReadReg(_currentInstruction.RegisterType1);
                _destinationIsMemory = true;
                return;

            case Instruction.EAddressMode.A16Reg:
            case Instruction.EAddressMode.D16Reg:
                _fetchData = ReadReg(_currentInstruction.RegisterType2);
                u16 low2 = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);
                u16 high2 = Bus.BusRead(_registers.PC ++);
                Emulator.EmulatorCycles(1);

                _memoryDestination = (u16) (low2 | (high2 << 8));
                _destinationIsMemory = true;
                return;

            case Instruction.EAddressMode.HLDReg:
                _fetchData = ReadReg(_currentInstruction.RegisterType2);
                _memoryDestination = ReadReg(_currentInstruction.RegisterType1);
                _destinationIsMemory = true;
                SetReg(Instruction.ERegisterType.HL, (u16) (ReadReg(Instruction.ERegisterType.HL) - 1));
                return;

            case Instruction.EAddressMode.HLIReg:
                _fetchData = ReadReg(_currentInstruction.RegisterType2);
                _memoryDestination = ReadReg(_currentInstruction.RegisterType1);
                _destinationIsMemory = true;
                SetReg(Instruction.ERegisterType.HL, (u16) (ReadReg(Instruction.ERegisterType.HL) + 1));
                return;

            case Instruction.EAddressMode.RegHLD:
                _fetchData = Bus.BusRead(ReadReg(_currentInstruction.RegisterType2));
                Emulator.EmulatorCycles(1);
                SetReg(Instruction.ERegisterType.HL, (u16) (ReadReg(Instruction.ERegisterType.HL) - 1));
                return;

            case Instruction.EAddressMode.RegHLI:
                _fetchData = Bus.BusRead(ReadReg(_currentInstruction.RegisterType2));
                Emulator.EmulatorCycles(1);
                SetReg(Instruction.ERegisterType.HL, (u16) (ReadReg(Instruction.ERegisterType.HL) + 1));
                return;
            
            
            default:
                Console.WriteLine("Unknown Address Mode");
                Environment.Exit(UNKNOWN_ADDRESS_MODE);
                return;
        }
    }


    public void Execute()
    {
        Delegate proc;
        if (_processors.TryGetValue(_currentInstruction.InstructionType, out proc))
        {
            if (proc is Action action)
            {
                action();
            }
        }
        else
        {
            Console.WriteLine("Execute():未找到對應指令方法");
            Environment.Exit(-5);
        }
        
    }


    private u16 Reverse(u16 n)
    {
        return (u16)(((n & 0xFF00) >> 8) | ((n & 0x00FF) << 8));
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
                return Reverse((u16)((_registers.F << 8) | _registers.A));

            case Instruction.ERegisterType.BC:
                return Reverse((u16)((_registers.C << 8) | _registers.B));

            case Instruction.ERegisterType.DE:
                return Reverse((u16)((_registers.E << 8) | _registers.D));

            case Instruction.ERegisterType.HL:
                return Reverse((u16)((_registers.L << 8) | _registers.H));

            
            case Instruction.ERegisterType.PC:
                return _registers.PC;

            case Instruction.ERegisterType.SP:
                return _registers.SP;

            default:
                return 0;
        }
    }


    private void SetReg(Instruction.ERegisterType regType, u16 value)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A:
                _registers.A = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.F:
                _registers.F = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.B:
                _registers.B = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.C:
                _registers.C = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.D:
                _registers.D = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.E:
                _registers.E = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.H:
                _registers.H = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.L:
                _registers.L = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.AF:
                _registers.A = (byte)(value & 0xFF);
                _registers.F = (byte)((value >> 8) & 0xFF);
                break;

            case Instruction.ERegisterType.BC:
                _registers.B = (byte)((value >> 8) & 0xFF);
                _registers.C = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.DE:
                _registers.D = (byte)((value >> 8) & 0xFF);
                _registers.E = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.HL:
                _registers.H = (byte)((value >> 8) & 0xFF);
                _registers.L = (byte)(value & 0xFF);
                break;

            case Instruction.ERegisterType.PC:
                _registers.PC = value;
                break;

            case Instruction.ERegisterType.SP:
                _registers.SP = value;
                break;

            default:
                throw new ArgumentException($"Unsupported register type: {regType}");
        }
    }



    private bool GetCPUFlag(string flag)
    {
        switch (flag)
        {
            case "Z":
                return Utils.GetBit(_registers.F, 7) == 1;
            
            case "C":
                return Utils.GetBit(_registers.F, 4) == 1;

            default:
                return false;
        }
    }


    private bool CheckCondition()
    {
        switch (_currentInstruction.ConditionType)
        {
            case Instruction.EConditionType.None:
                return true;

            case Instruction.EConditionType.Zero:
                return GetCPUFlag("Z");

            case Instruction.EConditionType.NotZero:
                return !GetCPUFlag("Z");

            case Instruction.EConditionType.Carry:
                return GetCPUFlag("C");

            case Instruction.EConditionType.NoCarry:
                return !GetCPUFlag("C");

            default:
                return false;
        }
    }


    private void SetCPUFlag(u8 z, u8 n, u8 h, u8 c)
    {
        if (z != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 7, z);
        }

        if (n != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 6, n);
        }

        if (h != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 5, h);
        }

        if (c != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 4, c);
        }
    }



    /*
        指令
    */
    private static void ProcNone()
    {
        Console.WriteLine("無效的指令");
        Environment.Exit(UNKNOWN_INSTRUCTION);
    }

    private void ProcNOP()
    {

    }

    private void ProcDI()
    {
        _instructionMasterEnable = false;
    }

    private void ProcLD()
    {
        if (_destinationIsMemory)
        {
            // 判斷要寫入的 value 是否為 16 bit
            if (_currentInstruction.RegisterType2 >= Instruction.ERegisterType.SP)
            {
                // 是 16 bit
                Emulator.EmulatorCycles(1);
                Bus.BusWrite16(_memoryDestination, _fetchData);
            }
            else
            {
                Bus.BusWrite(_memoryDestination, (u8) _fetchData);
            }
            return;
        }

        if (_currentInstruction.AddressMode == Instruction.EAddressMode.HLSPR)
        {
            // 針對 LD HL,SP+r8
            u8 hFlag = (u8) ((ReadReg(_currentInstruction.RegisterType2) & 0xF) + (_fetchData & 0xF) > 0xF ? 1 : 0);
            u8 cFlag = (u8) ((ReadReg(_currentInstruction.RegisterType2) & 0xFF) + (_fetchData & 0xFF) > 0xFF ? 1 : 0);
            SetCPUFlag(0, 0, hFlag, cFlag);
            SetReg(_currentInstruction.RegisterType1, (u16) (_currentInstruction.RegisterType2 + _fetchData));
            return;
        }

        SetReg(_currentInstruction.RegisterType1, _fetchData);
    }

    private void ProcXOR()
    {
        _registers.A ^= (u8) (_fetchData & 0xFF);
        SetCPUFlag((u8) (_registers.A == 0 ? 1 : 0), 0, 0, 0);
    }

    private void ProcJP()
    {
        if (CheckCondition())
        {
            _registers.PC = _fetchData;
            Emulator.EmulatorCycles(1);
        }
    }

    private void ProcLDH()
    {
        // LDH A,(a8)
        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.A)
        {
            SetReg(_currentInstruction.RegisterType1, Bus.BusRead((u16) (0xFF00 | _fetchData)));
        }
        // LDH (a8), A
        else 
        {
            Bus.BusWrite((u16) (0xFF00 | _fetchData), _registers.A);
        }

        Emulator.EmulatorCycles(1);
    }

    private void ProcPOP()
    {
        u16 low = Stack.Pop(ref _registers.SP);
        Emulator.EmulatorCycles(1);
        u16 high = Stack.Pop(ref _registers.SP);
        Emulator.EmulatorCycles(1);

        u16 value = (u16) ((high << 8) | low);

        SetReg(_currentInstruction.RegisterType1, value);

        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.AF)
        {
            SetReg(_currentInstruction.RegisterType1, (u16) (value & 0xFFF0));
        }
    }

    private void ProcPUSH()
    {
        u8 high = (u8) ((ReadReg(_currentInstruction.RegisterType1) >> 8) & 0xFF);
        Emulator.EmulatorCycles(1);
        Stack.Push(ref _registers.SP, high);

        u8 low = (u8) (ReadReg(_currentInstruction.RegisterType1) & 0xFF);
        Emulator.EmulatorCycles(1);
        Stack.Push(ref _registers.SP, low);

        Emulator.EmulatorCycles(1);
    }
}