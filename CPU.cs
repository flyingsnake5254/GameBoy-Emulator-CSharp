using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;


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
    private Instruction.ERegisterType[] _registerLookUp = 
    {
        Instruction.ERegisterType.B,
        Instruction.ERegisterType.C,
        Instruction.ERegisterType.D,
        Instruction.ERegisterType.E,
        Instruction.ERegisterType.H,
        Instruction.ERegisterType.L,
        Instruction.ERegisterType.HL,
        Instruction.ERegisterType.A,
    };


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
            { Instruction.EInstructionType.JR, new Action(ProcJR)},
            { Instruction.EInstructionType.CALL, new Action(ProcCALL)},
            { Instruction.EInstructionType.RST, new Action(ProcRST)},
            { Instruction.EInstructionType.RET, new Action(ProcRET)},
            { Instruction.EInstructionType.RETI, new Action(ProcRETI)},
            { Instruction.EInstructionType.LDH, new Action(ProcLDH)},
            { Instruction.EInstructionType.POP, new Action(ProcPOP)},
            { Instruction.EInstructionType.PUSH, new Action(ProcPUSH)},
            { Instruction.EInstructionType.ADD, new Action(ProcADD)},
            { Instruction.EInstructionType.INC, new Action(ProcINC)},
            { Instruction.EInstructionType.DEC, new Action(ProcDEC)},
            { Instruction.EInstructionType.SUB, new Action(ProcSUB)},
            { Instruction.EInstructionType.SBC, new Action(ProcSBC)},
            { Instruction.EInstructionType.ADC, new Action(ProcADC)},
            { Instruction.EInstructionType.AND, new Action(ProcAND)},
            { Instruction.EInstructionType.OR, new Action(ProcOR)},
            { Instruction.EInstructionType.CP, new Action(ProcCP)},
            { Instruction.EInstructionType.CB, new Action(ProcCB)},
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
                $"{Emulator.ticks, 0:X8} PC: {pc, 0:X4} " +
                $"{Instruction.GetInstructionName(_currentInstruction.InstructionType)} " + 
                $"({_currentOpcode, 0:X2}, {Bus.BusRead((u16) (pc + 1)), 0:X2}, {Bus.BusRead((u16) (pc + 2)), 0:X2}) " +
                $"    AF:{_registers.A, 0:X2}{_registers.F, 0:X2} BC:{_registers.B, 0:X2}{_registers.C, 0:X2} DE:{_registers.D, 0:X2}{_registers.E, 0:X2} HL:{_registers.H, 0:X2}{_registers.L, 0:X2} SP:{_registers.SP, 0:X4}" +
                $"    Flag:{(Utils.GetBit(_registers.F, 7) == 1 ? "Z" : "-")}{(Utils.GetBit(_registers.F, 6) == 1 ? "N" : "-")}{(Utils.GetBit(_registers.F, 5) == 1 ? "H" : "-")}{(Utils.GetBit(_registers.F, 4) == 1 ? "C" : "-")}");
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


    public u16 ReadReg(Instruction.ERegisterType regType)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A: return _registers.A;
            case Instruction.ERegisterType.F: return _registers.F;
            case Instruction.ERegisterType.B: return _registers.B;
            case Instruction.ERegisterType.C: return _registers.C;
            case Instruction.ERegisterType.D: return _registers.D;
            case Instruction.ERegisterType.E: return _registers.E;
            case Instruction.ERegisterType.H: return _registers.H;
            case Instruction.ERegisterType.L: return _registers.L;

            case Instruction.ERegisterType.AF: return (u16)((_registers.A << 8) | _registers.F);
            case Instruction.ERegisterType.BC: return (u16)((_registers.B << 8) | _registers.C);
            case Instruction.ERegisterType.DE: return (u16)((_registers.D << 8) | _registers.E);
            case Instruction.ERegisterType.HL: return (u16)((_registers.H << 8) | _registers.L);

            case Instruction.ERegisterType.PC: return _registers.PC;
            case Instruction.ERegisterType.SP: return _registers.SP;
            default: return 0;
        }
    }

    // 設置寄存器值
    public void SetReg(Instruction.ERegisterType regType, u16 value)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A:
                _registers.A = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.F:
                _registers.F = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.B:
                _registers.B = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.C:
                _registers.C = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.D:
                _registers.D = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.E:
                _registers.E = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.H:
                _registers.H = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.L:
                _registers.L = (u8)(value & 0xFF);
                break;

            case Instruction.ERegisterType.AF:
                _registers.A = (u8)((value >> 8) & 0xFF);
                _registers.F = (u8)(value & 0xFF);
                break;

            case Instruction.ERegisterType.BC:
                _registers.B = (u8)((value >> 8) & 0xFF);
                _registers.C = (u8)(value & 0xFF);
                break;

            case Instruction.ERegisterType.DE:
                _registers.D = (u8)((value >> 8) & 0xFF);
                _registers.E = (u8)(value & 0xFF);
                break;

            case Instruction.ERegisterType.HL:
                _registers.H = (u8)((value >> 8) & 0xFF);
                _registers.L = (u8)(value & 0xFF);
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


    public u8 ReadReg8(Instruction.ERegisterType regType)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A: return _registers.A;
            case Instruction.ERegisterType.F: return _registers.F;
            case Instruction.ERegisterType.B: return _registers.B;
            case Instruction.ERegisterType.C: return _registers.C;
            case Instruction.ERegisterType.D: return _registers.D;
            case Instruction.ERegisterType.E: return _registers.E;
            case Instruction.ERegisterType.H: return _registers.H;
            case Instruction.ERegisterType.L: return _registers.L;

            case Instruction.ERegisterType.HL: 
                return Bus.BusRead(ReadReg(Instruction.ERegisterType.HL));

            default: return 0;
        }
    }


    // 設置寄存器值
    public void SetReg8(Instruction.ERegisterType regType, u8 value)
    {
        switch (regType)
        {
            case Instruction.ERegisterType.A:
                _registers.A = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.F:
                _registers.F = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.B:
                _registers.B = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.C:
                _registers.C = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.D:
                _registers.D = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.E:
                _registers.E = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.H:
                _registers.H = (u8)(value & 0xFF);
                break;
            case Instruction.ERegisterType.L:
                _registers.L = (u8)(value & 0xFF);
                break;

            
            case Instruction.ERegisterType.HL:
                Bus.BusWrite(ReadReg(Instruction.ERegisterType.HL), value);
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


    private void SetCPUFlag(int z, int n, int h, int c)
    {
        if (z != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 7, (u8) z);
        }

        if (n != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 6, (u8) n);
        }

        if (h != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 5, (u8) h);
        }

        if (c != -1)
        {
            _registers.F = Utils.SetBit(_registers.F, 4, (u8) c);
        }
    }


    private void GotoAddress (u16 address, bool pushPC)
    {
        if (CheckCondition())
        {
            if (pushPC)
            {
                Emulator.EmulatorCycles(2);
                Stack.Push16(ref _registers.SP, _registers.PC);
            }

            _registers.PC = address;
            Emulator.EmulatorCycles(1);
        }
    }


    private bool Is16Bit(Instruction.ERegisterType registerType)
    {
        return registerType >= Instruction.ERegisterType.SP;
    }

    private Instruction.ERegisterType DecodeRegister(u8 reg)
    {
        if (reg > 0b111)
        {
            return Instruction.ERegisterType.NONE;
        }
        return _registerLookUp[reg];
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
            int hFlag = (int) ((ReadReg(_currentInstruction.RegisterType2) & 0xF) + (_fetchData & 0xF) > 0xF ? 1 : 0);
            int cFlag = (int) ((ReadReg(_currentInstruction.RegisterType2) & 0xFF) + (_fetchData & 0xFF) > 0xFF ? 1 : 0);
            SetCPUFlag(0, 0, hFlag, cFlag);
            SetReg(_currentInstruction.RegisterType1, (u16) (_currentInstruction.RegisterType2 + _fetchData));
            return;
        }

        SetReg(_currentInstruction.RegisterType1, _fetchData);
    }

    private void ProcXOR()
    {
        _registers.A ^= (u8) (_fetchData & 0xFF);
        SetCPUFlag((int) (_registers.A == 0 ? 1 : 0), 0, 0, 0);
    }

    private void ProcJP()
    {
        GotoAddress(_fetchData, false);
    }

    private void ProcJR()
    {
        u16 address = (u16) (_registers.PC + ((char) (_fetchData & 0xFF)));
        GotoAddress(address, false);
    }

    private void ProcCALL()
    {
        GotoAddress(_fetchData, true);
    }

    private void ProcRST()
    {
        GotoAddress(_currentInstruction.Param, true);
    }

    private void ProcRET()
    {
        if (_currentInstruction.ConditionType != Instruction.EConditionType.None)
        {
            Emulator.EmulatorCycles(1);
        }

        if (CheckCondition())
        {
            u16 low = Stack.Pop(ref _registers.SP);
            Emulator.EmulatorCycles(1);

            u16 high = Stack.Pop(ref _registers.SP);
            Emulator.EmulatorCycles(1);


            u16 value = (u16) ((high << 8) | low);
            _registers.PC = value;
            
            Emulator.EmulatorCycles(1);
        }
    }

    private void ProcRETI()
    {
        _instructionMasterEnable = true;
        ProcRET();
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

    private void ProcADD()
    {
        u32 value = (u32) (ReadReg(_currentInstruction.RegisterType1) + _fetchData);
        bool is16Bit = Is16Bit(_currentInstruction.RegisterType1);

        if (is16Bit)
        {
            Emulator.EmulatorCycles(1);
        }

        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.SP)
        {
            value = (u32) (ReadReg(_currentInstruction.RegisterType1) + ((char) _fetchData));
        }

        int z = (int) ((value & 0xFF) == 0 ? 1 : 0);
        int h = (int) ((ReadReg(_currentInstruction.RegisterType1) & 0xF) + (_fetchData & 0xF) >= 0x10 ? 1 : 0);
        int c = (int) (((ReadReg(_currentInstruction.RegisterType1) & 0xFF) + (_fetchData & 0xFF)) >= 0x100 ? 1 : 0);

        if (is16Bit)
        {
            z = -1;
            h = ((ReadReg(_currentInstruction.RegisterType1) & 0xFFF) + (_fetchData & 0xFFF)) >= 0x1000 ? 1 : 0;
            u32 n = ((u32) ReadReg(_currentInstruction.RegisterType1)) + ((u32) _fetchData);
            c = (n >= 0x10000 ? 1 : 0);
        }

        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.SP)
        {
            z = 0;
            h = ((ReadReg(_currentInstruction.RegisterType1) & 0xF) + (_fetchData & 0xF)) >= 0x10 ? 1 : 0; 
            c = ((int) (ReadReg(_currentInstruction.RegisterType1) & 0xFF) + (int) (_fetchData & 0xFF)) > 0x100 ? 1 : 0;
        }

        SetReg(_currentInstruction.RegisterType1, (u16) (value & 0xFFFF));
        SetCPUFlag(z, 0, h, c);
    }

    private void ProcINC()
    {
        u16 value = (u16) (ReadReg(_currentInstruction.RegisterType1) + 1);

        if (Is16Bit(_currentInstruction.RegisterType1))
        {
            Emulator.EmulatorCycles(1);
        }

        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.HL && _currentInstruction.AddressMode == Instruction.EAddressMode.Mem)
        {
            value = (u16) (Bus.BusRead(ReadReg(Instruction.ERegisterType.HL)) + 1);
            value &= 0xFF;
            Bus.BusWrite(ReadReg(Instruction.ERegisterType.HL), (u8) value);
        }
        else
        {
            SetReg(_currentInstruction.RegisterType1, value);
            value = ReadReg(_currentInstruction.RegisterType1);
        }

        if ((_currentOpcode & 0x03) == 0x03)
        {
            return;
        }

        SetCPUFlag(value == 0 ? 1 : 0, 0, (value & 0x0F) == 0 ? 1 : 0, -1);
    }

    private void ProcDEC()
    {
        u16 value = (u16) (ReadReg(_currentInstruction.RegisterType1) - 1);

        if (Is16Bit(_currentInstruction.RegisterType1))
        {
            Emulator.EmulatorCycles(1);
        }

        if (_currentInstruction.RegisterType1 == Instruction.ERegisterType.HL && _currentInstruction.AddressMode == Instruction.EAddressMode.Mem)
        {
            value = (u16) (Bus.BusRead(ReadReg(Instruction.ERegisterType.HL)) - 1);
            Bus.BusWrite(ReadReg(Instruction.ERegisterType.HL), (u8) value);
        }
        else
        {
            SetReg(_currentInstruction.RegisterType1, value);
            value = ReadReg(_currentInstruction.RegisterType1);
        }

        if ((_currentOpcode & 0x0B) == 0x0B)
        {
            return ;
        }

        SetCPUFlag(value == 0 ? 1 : 0, 1, (value & 0x0F) == 0x0F ? 1 : 0, -1);
    }

    private void ProcSUB()
    {
        u16 value = (u16) (ReadReg(_currentInstruction.RegisterType1) - _fetchData);

        int z = value == 0 ? 1 : 0;
        int h = ((((int) ReadReg(_currentInstruction.RegisterType1) & 0xF) - ((int) _fetchData & 0xF)) < 0) ? 1 : 0;
        int c = (((int) ReadReg(_currentInstruction.RegisterType1)) - ((int) _fetchData)) < 0 ? 1 : 0;

        SetReg(_currentInstruction.RegisterType1, value);
        SetCPUFlag(z, 1, h, c);
    }

    private void ProcSBC()
    {
        u8 value = (u8) (_fetchData + Utils.GetBit(_registers.F, 4));

        int z = ReadReg(_currentInstruction.RegisterType1) - value == 0 ? 1 : 0;
        int h = (((int) ReadReg(_currentInstruction.RegisterType1) & 0xF) - ((int) _fetchData & 0xF) - ((int) Utils.GetBit(_registers.F, 4))) < 0 ? 1 : 0;
        int c = (((int) ReadReg(_currentInstruction.RegisterType1)) - ((int) _fetchData) - ((int) Utils.GetBit(_registers.F, 4))) < 0 ? 1 : 0;

        SetReg(_currentInstruction.RegisterType1, (u16) (ReadReg(_currentInstruction.RegisterType1) - value));
        SetCPUFlag(z, 1, h, c);
    }

    private void ProcADC()
    {
        u16 u = _fetchData;
        u16 a = _registers.A;
        u16 c = (u16) (Utils.GetBit(_registers.F, 4));

        _registers.A = (u8) ((a + u + c) & 0xFF);

        SetCPUFlag(
            _registers.A == 0 ? 1 : 0, 
            0,
            (((a & 0xF) + (u & 0xF) + c) > 0xF) ? 1 : 0,
            a + u + c > 0xFF ? 1 : 0
        );
    }

    private void ProcAND()
    {
        _registers.A = (u8) (_registers.A & _fetchData);
        SetCPUFlag(_registers.A == 0 ? 1 : 0, 0, 1, 0);
    }

    private void ProcOR()
    {
        _registers.A = (u8) (_registers.A | (_fetchData & 0xFF));
        SetCPUFlag(_registers.A == 0 ? 1 : 0, 0, 0, 0);
    }

    private void ProcCP()
    {
        int n = (int) _registers.A - (int) _fetchData;
        SetCPUFlag(
            n == 0 ? 1 : 0, 
            1,
            (((int) (_registers.A & 0x0F)) - ((int) (_fetchData & 0x0F))) < 0 ? 1 : 0,
            n < 0 ? 1 : 0
        );
    }

    private void ProcCB()
    {
        u8 op = (u8) _fetchData;
        Instruction.ERegisterType reg = DecodeRegister((u8) (op & 0b111));
        u8 bit = (u8) ((op >> 3) & 0b111);
        u8 bitOp = (u8) ((op >> 6) & 0b11);
        u8 regValue = ReadReg8(reg);

        Emulator.EmulatorCycles(1);

        if (reg == Instruction.ERegisterType.HL)
        {
            Emulator.EmulatorCycles(2);
        }

        switch (bitOp)
        {
            case 1 :
                // BIT
                SetCPUFlag(
                    (regValue & (1 << bit)) == 1 ? 0 : 1,
                    0,
                    1,
                    -1
                );
                return ;

            case 2 :
                // RST
                regValue = (u8) (regValue & (~(1 << bit)));
                SetReg8(reg, regValue);
                return ;

            case 3 :
                // SET
                regValue = (u8) (regValue | (1 << bit));
                SetReg8(reg, regValue);
                return ;
        }

        u8 flagC = (u8) (Utils.GetBit(_registers.F, 4));

        switch (bit)
        {
            case 0 :
                // RLC
                int setC = 0;
                u8 result = (u8) ((regValue << 1) & 0xFF);

                if ((regValue & (1 << 7)) != 0)
                {
                    result = (u8) (result | 1);
                    setC = 1;
                }

                SetReg8(reg, result);
                SetCPUFlag(result == 0 ? 1 : 0, 0, 0, setC);
                return ;

            case 1 :
                // RRC
                u8 old = regValue;
                regValue = (u8) (regValue >> 1);

                SetReg8(reg, regValue);
                SetCPUFlag(regValue == 1 ? 0 : 1, 0, 0, (old & 1) == 0 ? 0 : 1);
                return ;

            case 2 :
                // RL
                u8 old2 = regValue;
                regValue = (u8) (regValue << 1);
                regValue = (u8) (regValue | flagC);

                SetReg8(reg, regValue);
                SetCPUFlag(regValue == 0 ? 0 : 1, 0, 0, (old2 & 0x80) == 1 ? 1 : 0);
                return ;

            case 3 :
                // RR
                u8 old3 = regValue;
                regValue = (u8) (regValue >> 1);

                regValue = (u8) (regValue | (flagC << 7));

                SetReg8(reg, regValue);
                SetCPUFlag(regValue == 0 ? 1 : 0, 0, 0, (old3 & 1) == 0 ? 0 : 1);
                return ;

            case 4 :
                // SLA
                u8 old4 = regValue;
                regValue = (u8) (regValue << 1);

                SetReg8(reg, regValue);
                SetCPUFlag(
                    regValue == 0 ? 1 : 0,
                    0,
                    0,
                    (old4 & 0x80) == 0 ? 0 : 1
                );
                return ;

            case 5 :
                // SRA
                u8 u = (u8) (((System.SByte) regValue) >> 1);
                SetReg8(reg, u);
                SetCPUFlag(u == 0 ? 1 : 0, 0, 0, (regValue & 1) == 0 ? 0 : 1);
                return ;

            case 6 :
                // SWAP
                regValue = (u8) (((regValue & 0xF0) >> 4) | ((regValue & 0xF) << 4));
                SetReg8(reg, regValue);
                SetCPUFlag(regValue == 0 ? 1 : 0, 0, 0, 0);
                return ;

            case 7 :
                // SRL
                u8 u2 = (u8) (regValue >> 1);
                SetReg8(reg, u2);
                SetCPUFlag(u2 == 0 ? 1 : 0, 0, 0, (regValue & 1) == 0 ? 0 : 1);
                return ;
        }

        Console.WriteLine($"Invalid CB : {op, 0:X2}");
        return ;
    }


}