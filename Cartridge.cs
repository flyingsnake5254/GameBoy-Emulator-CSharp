using System.Text;
using Global.GCartridge;

public class Cartridge
{
    // 卡匣 Header
    public string Title { get; private set; }
    public byte CartridgeType { get; private set; }
    public int ROMSize { get; private set; }
    public int RAMSize { get; private set; }
    
    private string[] CartridgeTypes = {
        "ROM ONLY", "MBC1", "MBC1+RAM", "MBC1+RAM+BATTERY", "MBC2",
        "MBC2+BATTERY", "ROM+RAM", "ROM+RAM+BATTERY", "MMM01", "MMM01+RAM",
        "MMM01+RAM+BATTERY", "MBC3+TIMER+BATTERY", "MBC3+TIMER+RAM+BATTERY", "MBC3", "MBC3+RAM",
        "MBC3+RAM+BATTERY", "MBC5", "MBC5+RAM", "MBC5+RAM+BATTERY", "MBC5+RUMBLE",
        "MBC5+RUMBLE+RAM", "MBC5+RUMBLE+RAM+BATTERY", "MBC6", "MBC7+SENSOR+RUMBLE+RAM+BATTERY", "POCKET CAMERA",
        "BANDAI TAMA5", "HuC3", "HuC1+RAM+BATTERY"
    };

    private int[] RAMSizes = { 0, -1, 8, 32, 128, 64 };

    // Old Licensee Code
    private string[] OldLicenseeCodes = {
        "None", "Nintendo", "Capcom", "HOT-B", "Jaleco",
        "Coconuts Japan", "Elite Systems", "	EA (Electronic Arts)", "Hudson Soft", "ITC Entertainment",
        "Yanoman", "Japan Clary", "Virgin Games Ltd.", "PCM Complete", "San-X",
        "Kemco", "SETA Corporation", "Infogrames", "Nintendo", "Bandai",
        "--", "Konami", "HectorSoft", "Capcom", "Banpresto",
        "Entertainment Interactive (stub)", "Gremlin", "Ubi Soft", "Atlus", "Malibu Interactive",
        "Angel", "Spectrum HoloByte", "Irem", "	Virgin Games Ltd.", "Malibu Interactive",
        "U.S. Gold", "Absolute", "Acclaim Entertainment", "Activision", "	Sammy USA Corporation",
        "GameTek", "Park Place", "LJN", "Matchbox", "Milton Bradley Company",
        "Mindscape", "Romstar", "Naxat Soft", "Tradewest", "Titus Interactive",
        "Virgin Games Ltd.", "Ocean Software", "EA (Electronic Arts)", "Elite Systems", "Electro Brain",
        "Infogrames", "Interplay Entertainment", "Broderbund", "Sculptured Software", "The Sales Curve Limited",
        "THQ", "Accolade", "Triffix Entertainment", "MicroProse", "Kemco",
        "Misawa Entertainment", "LOZC G.", "Tokuma Shoten", "Bullet-Proof Software", "Vic Tokai Corp.",
        "Ape Inc.", "I’Max", "Chunsoft Co.", "Video System", "Tsubaraya Productions",
        "Varie", "Yonezawa", "Kemco", "Arc", "Nihon Bussan",
        "Tecmo", "Imagineer", "Banpresto", "Nova", "Hori Electric",
        "Bandai", "Konami", "Kawada", "	Takara", "Technos Japan",
        "Broderbund", "Toei Animation", "Toho", "Namco", "Acclaim Entertainment(B0)",
        "", "", "", "", "",
        "", "", "", "", "",
        "", "", "", "", "",
        "", "", "", "", "",
    };

    public bool Load(string path)
    {
        try
        {
            byte[] romData = File.ReadAllBytes(path);
            ParseHeader(romData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }


    public void ParseHeader(byte[] romData)
    {
        // Title
        if (romData[(int) Header.CGBFlag] == 0x80 || romData[(int) Header.CGBFlag] == 0xC0)
        {
            // Title (11 bytes) : 0x0134 ~ 0x013E
            Title = Encoding.ASCII.GetString(romData, (int) Header.TitleStart, 11);
        }
        else
        {
            // Title (15 bytes) : 0x0134 ~ 0x0142
            Title = Encoding.ASCII.GetString(romData, (int) Header.TitleStart, 15);
        }


        // Cartridge Type
        CartridgeType = romData[(int) Header.CartridgeType];


        // ROM Size
        // ROM size = 32 KiB × (1 << <value>)
        // ref : https://gbdev.io/pandocs/The_Cartridge_Header.html#0148--rom-size
        ROMSize = 32 * (1 << romData[(int) Header.ROMSize]);


        // RAM Size
        RAMSize = RAMSizes[romData[(int) Header.RAMSize]];

        // 輸出卡匣 Header 到終端
        Console.WriteLine($"{"Title", -20} : {Title, -15}");
        Console.WriteLine($"{"Cartridge Type", -20} : {CartridgeTypes[CartridgeType], -15}");
        Console.WriteLine($"{"ROM Size", -20} : {ROMSize + (romData[(int) Header.ROMSize] <= 0x04 ? "KiB" : "MiB"), -15}");
        Console.WriteLine($"{"RAM Size", -20} : {RAMSize + "KiB", -15}");
    }
}