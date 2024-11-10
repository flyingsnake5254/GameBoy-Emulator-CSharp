using u8 = System.Byte;
using u16 = System.UInt16;

public class Stack
{
    public static void Push (ref u16 SP, u8 value)
    {
        SP --;
        Bus.BusWrite(SP, value);
    }

    public static u8 Pop (ref u16 SP)
    {
        return Bus.BusRead(SP ++);
    }


    public static void Push16 (ref u16 SP, u16 value)
    {
        // 先 push 高8位
        Push(ref SP, (u8) ((value >> 8) & 0xFF));
        // 再 push 低8位
        Push(ref SP, (u8) (value & 0xFF));
    }

    public static u16 Pop16 (ref u16 SP)
    {
        u8 low = Pop(ref SP);
        u8 high = Pop(ref SP);
        return (u16) ((high << 8) | low);
    }
}