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
        { 0x00, new Instruction(EInstructionType.NOP, EAddressMode.Implicit)},
        { 0x05, new Instruction(EInstructionType.DEC, EAddressMode.Reg, ERegisterType.B)},
        { 0x0E, new Instruction(EInstructionType.LD, EAddressMode.RegD8, ERegisterType.C)},
        { 0x3C, new Instruction(EInstructionType.INC, EAddressMode.Reg, ERegisterType.A)},
        { 0xAF, new Instruction(EInstructionType.XOR, EAddressMode.Reg, ERegisterType.A)},
        { 0xC3, new Instruction(EInstructionType.JP, EAddressMode.D16)},
        { 0xF3, new Instruction(EInstructionType.DI, EAddressMode.Implicit)}
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