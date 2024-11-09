using u8 = System.Byte;
using u16 = System.Int16;

public class Bus
{
    public static u8 BusRead(u16 address)
    {
        // ROM
        if (address < 0x8000)
        {
            return Cartridge.CartridgeRead(address);
        }
        Console.WriteLine("BusRead() 尚未實現");
        return 0;
    }


    public static void BusWrite(u16 address, u8 value)
    {
        Cartridge.CartridgeWrite(address, value);
    }


    public static u16 BusRead16(u16 address)
    {
        u16 lo = BusRead(address);
        u16 hi = BusRead((u16)(address + 1));

        return (u16)(lo | (hi << 8));
    }

    public static void BusWrite16(u16 address, u16 value)
    {
        BusWrite((u16)(address + 1), (u8)((value >> 8) & 0xFF));
        BusWrite(address, (u8)(value & 0xFF));
    }

}