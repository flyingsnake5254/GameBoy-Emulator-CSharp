using u8 = System.Byte;
using u16 = System.UInt16;

// ---
// ROM0, ROMX
// 0x0000 - 0x3FFF : ROM Bank 0
// 0x4000 - 0x7FFF : ROM Bank 1 - Switchable
// ---
// VRAM
// 0x8000 - 0x97FF : CHR RAM
// 0x9800 - 0x9BFF : BG Map 1
// 0x9C00 - 0x9FFF : BG Map 2
// ---
// SRAM
// 0xA000 - 0xBFFF : Cartridge RAM
// ---
// WRAM0
// 0xC000 - 0xCFFF : RAM Bank 0
// 0xD000 - 0xDFFF : RAM Bank 1-7 - switchable - Color only
// ---
// ECHO
// 0xE000 - 0xFDFF : Reserved - Echo RAM
// ---
// OAM
// 0xFE00 - 0xFE9F : Object Attribute Memory
// ---
// UNUSED
// 0xFEA0 - 0xFEFF : Reserved - Unusable
// ---
// I/O Registers
// 0xFF00 - 0xFF7F : I/O Registers
// ---
// HRAM
// 0xFF80 - 0xFFFE : Zero Page
// ---
// 0xFFFF -> IE Register


public class Bus
{
    public static u8 BusRead(u16 address)
    {
        // ROM
        if (address < 0x8000)
        {
            return Cartridge.CartridgeRead(address);
        }
        else if (address < 0xA000)
        {
            // VRAM (Char/Map Data)
            Console.Write($"BusRead() - VRAM(0x8000-0x9FFF):{address, 0:X4} - 尚未實現");
            return 0;
        }
        else if (address < 0xC000)
        {
            // SRAM (Cartridge RAM)
            return Cartridge.CartridgeRead(address);
        }
        else if (address < 0xE000)
        {
            // WRAM (Working RAM)
            return RAM.WRAMRead(address);
        }
        else if (address < 0xFE00)
        {
            // Reserved Echo RAM
            return 0;
        }
        else if (address < 0xFEA0)
        {
            // OAM
            Console.Write($"BusRead() - OAM(0xFE00-0xFE9F):{address, 0:X4} - 尚未實現");
            // Environment.Exit(-1);
            return 0;
        }
        else if (address < 0xFF00)
        {
            // UNUSED
            return 0;
        }
        else if (address < 0xFF80)
        {
            // I/O Registers
            Console.Write($"BusRead() - I/O Registers(0xFF00-0xFF7F):{address, 0:X4} - 尚未實現");
            // Environment.Exit(-1);
            return 0;
        }
        else if (address < 0xFFFF)
        {
            // HRAM
            return RAM.HRAMRead(address);
        }
        else
        {
            // address == 0xFFFF
            // IE Register (CPU Enable Register)
            return CPU.IERegister;
        }
    }


    public static void BusWrite(u16 address, u8 value)
    {
        if (address < 0x8000)
        {
            Cartridge.CartridgeWrite(address, value);
            return;
        }
        else if (address < 0xA000)
        {
            // VRAM (Char/Map Data)
            Console.Write($"BusWrite() - VRAM(0x8000-0x9FFF):{address, 0:X4} - 尚未實現");
            // Environment.Exit(-1);
        }
        else if (address < 0xC000)
        {
            // SRAM (Cartridge RAM)
            Cartridge.CartridgeWrite(address, value);
        }
        else if (address < 0xE000)
        {
            // WRAM (Working RAM)
            RAM.WRAMWrite(address, value);
        }
        else if (address < 0xFE00)
        {
            // Reserved Echo RAM
            return ;
        }
        else if (address < 0xFEA0)
        {
            // OAM
            Console.Write($"BusWrite() - OAM(0xFE00-0xFE9F):{address, 0:X4} - 尚未實現");
            // Environment.Exit(-1);
        }
        else if (address < 0xFF00)
        {
            // UNUSED
            return ;
        }
        else if (address < 0xFF80)
        {
            // I/O Registers
            Console.Write($"BusWrite() - I/O Registers(0xFF00-0xFF7F):{address, 0:X4} - 尚未實現");
            // Environment.Exit(-1);
        }
        else if (address < 0xFFFF)
        {
            // HRAM
            RAM.HRAMWrite(address, value);
        }
        else
        {
            // address == 0xFFFF
            // IE Register (CPU Enable Register)
            CPU.IERegister = value;
        }
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