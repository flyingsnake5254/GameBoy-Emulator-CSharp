using u8 = System.Byte;
using u16 = System.UInt16;

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
}