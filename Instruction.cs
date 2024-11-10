using u8 = System.Byte;


public class Instruction
{
    public enum EConditionType
    {
        None, // 無條件，指令無條件執行
        NotZero, // 非零，當零旗標（Z）為 0 時條件為真
        Zero, // 零，當零旗標（Z）為 1 時條件為真
        NoCarry, // 無進位，當進位旗標（C）為 0 時條件為真
        Carry, // 進位，當進位旗標（C）為 1 時條件為真
    }


    public enum EInstructionType
    {
        // CPU Instruction
        NONE, STOP, JR, LD, ADD, SUB, AND, OR, RET, LDH,
        POP, JP, INC, CALL, DEC, PUSH, HALT, RLCA, RLA, DI, 
        DAA, SCF, RST, ADC, SBC, XOR, CP, EI, RRCA, RRA,
        CPL, CCF, NOP, JPHL, RETI, CB, 
        // Prefix CB
        RLC, RRC, RL, RR, SLA, SRA, SWAP, SRL, BIT, RES, 
        SET, 
        // 無效或錯誤的指令
        ERR
    }


    /*
        (a16) --> A16
        A16 --> D16
        r8 --> D8
        HL, SP+r8 --> HLSPR
        (HL+) --> HLI
        (HL-) --> HLD
        (XX) --> Mem
    */
    public enum EAddressMode
    {
        Implicit, D8, RegReg, MemReg, Reg, 
        A8Reg, RegA8, HLSPR, RegA16, RegD16, 
        D16, RegMem, Mem, RegD8, MemD8, 
        A16Reg, D16Reg, HLDReg, HLIReg, RegHLD, 
        RegHLI
    }


    public enum ERegisterType
    {
        NONE,
        A, F, B, C, D, E, H, L, SP, PC,
        AF, BC, DE, HL
    }


    // 成員變數
    public EInstructionType InstructionType { get; set; }
    public EAddressMode AddressMode { get; set; }
    public ERegisterType RegisterType1 { get; set; }
    public ERegisterType RegisterType2 { get; set; }
    public EConditionType ConditionType { get; set; }
    public u8 Param { get; set; }


    // 建構子
    public Instruction(
        EInstructionType instructionType,
        EAddressMode addressMode,
        ERegisterType registerType1 = ERegisterType.NONE,
        ERegisterType registerType2 = ERegisterType.NONE,
        EConditionType conditionType = EConditionType.None,
        u8 param = 0
    )
    {
        InstructionType = instructionType;
        AddressMode = addressMode;
        RegisterType1 = registerType1;
        RegisterType2 = registerType2;
        ConditionType = conditionType;
        Param = param;
    }


    // 定義指令集
    public static Dictionary<u8, Instruction> Instructions = new Dictionary<u8, Instruction> 
    {
        // 0x0-
        { 0x00, new Instruction(EInstructionType.NOP, EAddressMode.Implicit)},
        { 0x01, new Instruction(EInstructionType.LD, EAddressMode.RegD16, ERegisterType.BC)},
        { 0x02, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.BC, ERegisterType.A)},
        // { 0x03, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.BC)},
        // { 0x04, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.B)},
        { 0x05, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.B)},
        { 0x06, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.B)},
        // { 0x07, new Instruction(EInstructionType.RLCA, EAddressMode.Implicit)},
        { 0x08, new Instruction(EInstructionType.LD, EAddressMode.A16Reg, ERegisterType.NONE, ERegisterType.SP)},
        // { 0x09, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.HL, ERegisterType.BC)},
        { 0x0A, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.A, ERegisterType.BC)},
        // { 0x0B, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.BC)},
        // { 0x0C, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.C)},
        // { 0x0D, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.C)},
        { 0x0E, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.C)},
        // { 0x0F, new Instruction(EInstructionType.RRCA, EAddressMode.Implicit)},

        // 0x1-
        // { 0x10, new Instruction(EInstructionType.STOP, EAddressMode.Implicit)},
        { 0x11, new Instruction(EInstructionType.LD, EAddressMode.RegD16, ERegisterType.DE)},
        { 0x12, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.DE, ERegisterType.A)},
        // { 0x13, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.DE)},
        // { 0x14, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.D)},
        { 0x15, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.D)},
        { 0x16, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.D)},
        // { 0x17, new Instruction(EInstructionType.RLA, EAddressMode.Implicit)},
        { 0x18, new Instruction(EInstructionType.JR, EAddressMode.D8)},
        // { 0x19, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.HL, ERegisterType.DE)},
        { 0x1A, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.A, ERegisterType.DE)},
        // { 0x1B, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.DE)},
        // { 0x1C, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.E)},
        // { 0x1D, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.E)},
        { 0x1E, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.E)},
        // { 0x1F, new Instruction(EInstructionType.RRA, EAddressMode.Implicit)},

        // 0x2-
        { 0x20, new Instruction(EInstructionType.JR, EAddressMode.D8, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NotZero)},
        { 0x21, new Instruction(EInstructionType.LD, EAddressMode.RegD16, ERegisterType.HL)},
        { 0x22, new Instruction(EInstructionType.LD, EAddressMode.HLIReg, ERegisterType.HL, ERegisterType.A)},
        // { 0x23, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.HL)},
        // { 0x24, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.H)},
        { 0x25, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.H)},
        { 0x26, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.H)},
        // { 0x27, new Instruction(EInstructionType.DAA, EAddressMode.Implicit)},
        { 0x28, new Instruction(EInstructionType.JR, EAddressMode.D8, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Zero)},
        // { 0x29, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.HL, ERegisterType.HL)},
        { 0x2A, new Instruction(EInstructionType.LD, EAddressMode.RegHLI, ERegisterType.A, ERegisterType.HL)},
        // { 0x2B, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.HL)},
        // { 0x2C, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.L)},
        // { 0x2D, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.L)},
        { 0x2E, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.L)},
        // { 0x2F, new Instruction(EInstructionType.CPL, EAddressMode.Implicit)},

        // 0x3-
        { 0x30, new Instruction(EInstructionType.JR, EAddressMode.D8, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0x31, new Instruction(EInstructionType.LD, EAddressMode.RegD16, ERegisterType.SP)},
        { 0x32, new Instruction(EInstructionType.LD, EAddressMode.HLDReg, ERegisterType.HL, ERegisterType.A)},
        // { 0x33, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.SP)},
        // { 0x34, new Instruction(EInstructionType.INC, EAddressMode.Mem, ERegisterType.HL)},
        { 0x35, new Instruction(EInstructionType.DEC, EAddressMode.Mem, ERegisterType.HL)},
        { 0x36, new Instruction(EInstructionType.LD, EAddressMode.MemD8, ERegisterType.HL)},
        // { 0x37, new Instruction(EInstructionType.SCF, EAddressMode.Implicit)},
        { 0x38, new Instruction(EInstructionType.JR, EAddressMode.D8, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Carry)},
        // { 0x39, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.HL, ERegisterType.SP)},
        { 0x3A, new Instruction(EInstructionType.LD, EAddressMode.RegHLD, ERegisterType.A, ERegisterType.HL)},
        // { 0x3B, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.SP)},
        // { 0x3C, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.A)},
        // { 0x3D, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.A)},
        { 0x3E, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.A)},
        // { 0x3F, new Instruction(EInstructionType.CCF, EAddressMode.Implicit)},

        // 0x4-
        { 0x40, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.B)},
        { 0x41, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.C)},
        { 0x42, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.D)},
        { 0x43, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.E)},
        { 0x44, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.H)},
        { 0x45, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.L)},
        { 0x46, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.B, ERegisterType.HL)},
        { 0x47, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.B, ERegisterType.A)},
        { 0x48, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.B)},
        { 0x49, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.C)},
        { 0x4A, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.D)},
        { 0x4B, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.E)},
        { 0x4C, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.H)},
        { 0x4D, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.L)},
        { 0x4E, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.C, ERegisterType.HL)},
        { 0x4F, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.C, ERegisterType.A)},

        // 0x5-
        { 0x50, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.B)},
        { 0x51, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.C)},
        { 0x52, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.D)},
        { 0x53, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.E)},
        { 0x54, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.H)},
        { 0x55, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.L)},
        { 0x56, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.D, ERegisterType.HL)},
        { 0x57, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.D, ERegisterType.A)},
        { 0x58, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.B)},
        { 0x59, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.C)},
        { 0x5A, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.D)},
        { 0x5B, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.E)},
        { 0x5C, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.H)},
        { 0x5D, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.L)},
        { 0x5E, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.E, ERegisterType.HL)},
        { 0x5F, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.E, ERegisterType.A)},

        // 0x6-
        { 0x60, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.B)},
        { 0x61, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.C)},
        { 0x62, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.D)},
        { 0x63, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.E)},
        { 0x64, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.H)},
        { 0x65, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.L)},
        { 0x66, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.H, ERegisterType.HL)},
        { 0x67, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.H, ERegisterType.A)},
        { 0x68, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.B)},
        { 0x69, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.C)},
        { 0x6A, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.D)},
        { 0x6B, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.E)},
        { 0x6C, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.H)},
        { 0x6D, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.L)},
        { 0x6E, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.L, ERegisterType.HL)},
        { 0x6F, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.L, ERegisterType.A)},

        // 0x7-
        { 0x70, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.B)},
        { 0x71, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.C)},
        { 0x72, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.D)},
        { 0x73, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.E)},
        { 0x74, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.H)},
        { 0x75, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.L)},
        { 0x76, new Instruction(EInstructionType.HALT, EAddressMode.Implicit)},
        { 0x77, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.HL, ERegisterType.A)},
        { 0x78, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        { 0x79, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        { 0x7A, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        { 0x7B, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        { 0x7C, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        { 0x7D, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        { 0x7E, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        { 0x7F, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},

        // 0x8-
        // { 0x80, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0x81, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0x82, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0x83, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0x84, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0x85, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0x86, new Instruction(EInstructionType.ADD, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0x87, new Instruction(EInstructionType.ADD, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},
        // { 0x88, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0x89, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0x8A, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0x8B, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0x8C, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0x8D, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0x8E, new Instruction(EInstructionType.ADC, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0x8F, new Instruction(EInstructionType.ADC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},

        // 0x9-
        // { 0x90, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0x91, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0x92, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0x93, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0x94, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0x95, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0x96, new Instruction(EInstructionType.SUB, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0x97, new Instruction(EInstructionType.SUB, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},
        // { 0x98, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0x99, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0x9A, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0x9B, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0x9C, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0x9D, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0x9E, new Instruction(EInstructionType.SBC, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0x9F, new Instruction(EInstructionType.SBC, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},

        // 0xA-
        // { 0xA0, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0xA1, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0xA2, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0xA3, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0xA4, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0xA5, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0xA6, new Instruction(EInstructionType.AND, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0xA7, new Instruction(EInstructionType.AND, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},
        // { 0xA8, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0xA9, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0xAA, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0xAB, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0xAC, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0xAD, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0xAE, new Instruction(EInstructionType.XOR, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        { 0xAF, new Instruction(EInstructionType.XOR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},

        // 0xB-
        // { 0xB0, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0xB1, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0xB2, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0xB3, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0xB4, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0xB5, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0xB6, new Instruction(EInstructionType.OR, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0xB7, new Instruction(EInstructionType.OR, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},
        // { 0xB8, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.B)},
        // { 0xB9, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.C)},
        // { 0xBA, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.D)},
        // { 0xBB, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.E)},
        // { 0xBC, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.H)},
        // { 0xBD, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.L)},
        // { 0xBE, new Instruction(EInstructionType.CP, EAddressMode.RegMem, ERegisterType.A, ERegisterType.HL)},
        // { 0xBF, new Instruction(EInstructionType.CP, EAddressMode.RegReg, ERegisterType.A, ERegisterType.A)},

        // 0xC-
        { 0xC0, new Instruction(EInstructionType.RET, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xC1, new Instruction(EInstructionType.POP, EAddressMode.Reg, ERegisterType.BC)},
        // { 0xC2, new Instruction(EInstructionType.JP, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xC3, new Instruction(EInstructionType.JP, EAddressMode.D16)},
        { 0xC4, new Instruction(EInstructionType.CALL, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xC5, new Instruction(EInstructionType.PUSH, EAddressMode.Reg, ERegisterType.BC)},
        // { 0xC6, new Instruction(EInstructionType.ADD, EAddressMode.RegD8, ERegisterType.A)},
        { 0xC7, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x00)},
        { 0xC8, new Instruction(EInstructionType.RET, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Zero)},
        { 0xC9, new Instruction(EInstructionType.RET, EAddressMode.Implicit)},
        // { 0xCA, new Instruction(EInstructionType.JP, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Zero)},
        // { 0xCB, new Instruction(EInstructionType.CB, EAddressMode.D8)},
        { 0xCC, new Instruction(EInstructionType.CALL, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Zero)},
        { 0xCD, new Instruction(EInstructionType.CALL, EAddressMode.D16)},
        // { 0xCE, new Instruction(EInstructionType.ADC, EAddressMode.RegD8, ERegisterType.A)},
        { 0xCF, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x08)},

        // 0xD-
        { 0xD0, new Instruction(EInstructionType.RET, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xD1, new Instruction(EInstructionType.POP, EAddressMode.Reg, ERegisterType.DE)},
        // { 0xD2, new Instruction(EInstructionType.JP, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xD4, new Instruction(EInstructionType.CALL, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.NoCarry)},
        { 0xD5, new Instruction(EInstructionType.PUSH, EAddressMode.Reg, ERegisterType.DE)},
        // { 0xD6, new Instruction(EInstructionType.SUB, EAddressMode.RegD8, ERegisterType.A)},
        { 0xD7, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x10)},
        { 0xD8, new Instruction(EInstructionType.RET, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Carry)},
        { 0xD9, new Instruction(EInstructionType.RETI, EAddressMode.Implicit)},
        // { 0xDA, new Instruction(EInstructionType.JP, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Carry)},
        { 0xDC, new Instruction(EInstructionType.CALL, EAddressMode.D16, ERegisterType.NONE, ERegisterType.NONE, EConditionType.Carry)},
        // { 0xDE, new Instruction(EInstructionType.SBC, EAddressMode.RegD8, ERegisterType.A)},
        { 0xDF, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x18)},

        // 0xE-
        { 0xE0, new Instruction(EInstructionType.LDH, EAddressMode.A8Reg, ERegisterType.NONE, ERegisterType.A)},
        { 0xE1, new Instruction(EInstructionType.POP, EAddressMode.Reg, ERegisterType.HL)},
        { 0xE2, new Instruction(EInstructionType.LD, EAddressMode.MemReg, ERegisterType.C, ERegisterType.A)},
        { 0xE5, new Instruction(EInstructionType.PUSH, EAddressMode.Reg, ERegisterType.HL)},
        // { 0xE6, new Instruction(EInstructionType.AND, EAddressMode.RegD8, ERegisterType.A)},
        { 0xE7, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x20)},
        // { 0xE8, new Instruction(EInstructionType.ADD, EAddressMode.RegD8, ERegisterType.SP)},
        // { 0xE9, new Instruction(EInstructionType.JP, EAddressMode.Reg, ERegisterType.HL)},
        { 0xEA, new Instruction(EInstructionType.LD, EAddressMode.A16Reg, ERegisterType.NONE, ERegisterType.A)},
        // { 0xEE, new Instruction(EInstructionType.XOR, EAddressMode.RegD8, ERegisterType.A)},
        { 0xEF, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x28)},

        // 0xF-
        { 0xF0, new Instruction(EInstructionType.LDH, EAddressMode.RegA8, ERegisterType.A)},
        { 0xF1, new Instruction(EInstructionType.POP, EAddressMode.Reg, ERegisterType.AF)},
        { 0xF2, new Instruction(EInstructionType.LD, EAddressMode.RegMem, ERegisterType.A, ERegisterType.C)},
        { 0xF3, new Instruction(EInstructionType.DI, EAddressMode.Implicit)},
        { 0xF5, new Instruction(EInstructionType.PUSH, EAddressMode.Reg, ERegisterType.AF)},
        // { 0xF6, new Instruction(EInstructionType.OR, EAddressMode.RegD8, ERegisterType.A)},
        { 0xF7, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x30)},
        // { 0xF8, new Instruction(EInstructionType.LD, EAddressMode.HLSPR, ERegisterType.HL, ERegisterType.SP)},
        // { 0xF9, new Instruction(EInstructionType.LD, EAddressMode.RegReg, ERegisterType.SP, ERegisterType.HL)},
        { 0xFA, new Instruction(EInstructionType.LD, EAddressMode.RegA16, ERegisterType.A)},
        // { 0xFB, new Instruction(EInstructionType.EI, EAddressMode.Implicit)},
        // { 0xFE, new Instruction(EInstructionType.CP, EAddressMode.RegD8, ERegisterType.A)},
        { 0xFF, new Instruction(EInstructionType.RST, EAddressMode.Implicit, ERegisterType.NONE, ERegisterType.NONE, EConditionType.None, 0x38)},
    };


    public static Instruction InstructionByOpcode(u8 opcode)
    {
        if (Instructions.TryGetValue(opcode, out Instruction instruction))
        {
            return instruction;
        }
        return null;
    }


    public static string GetInstructionName(EInstructionType instructionType)
    {
        return instructionType.ToString();
    }


}