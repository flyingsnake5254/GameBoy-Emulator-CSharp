using u8 = System.Byte;
using u16 = System.UInt16;

/*
WRAM : 0xC000 ~ 0xDFFF
HRAM : 0xFF80 ~ 0xFFFE
*/
public class RAM
{
    private static u8[] WRAM = new u8[0x2000];
    private static u8[] HRAM = new u8[0x80];

    public static u8 WRAMRead(u16 address)
    {
        address -= 0xC000;
        if (address >= 0x2000)
        {
            Console.WriteLine($"無效的 WRAM Address - {(address + 0xC000), 0:X8}");
            Environment.Exit(-1);
        }
        return WRAM[address];
    }


    public static void WRAMWrite(u16 address, u8 value)
    {
        address -= 0xC000;
        if (address >= 0x2000)
        {
            Console.WriteLine($"無效的 WRAM Address - {(address + 0xC000), 0:X8}");
            Environment.Exit(-1);
        }
        WRAM[address] = value;
    }


    public static u8 HRAMRead(u16 address)
    {
        address -= 0xFF80;
        return HRAM[address];
    }


    public static void HRAMWrite(u16 address, u8 value)
    {
        address -= 0xFF80;
        HRAM[address] = value;
    }
}