using System.Text;
using Global.GCartridge;

public class Cartridge
{
    // 卡匣 Header
    public string Title { get; private set; }
    public byte CartridgeType { get; private set; }
    public int ROMSize { get; private set; }
    public int RAMSize { get; private set; }
    public byte OldLicenseeCode { get; private set; }
    public string Publisher { get; private set; }
    public byte ROMVersion { get; private set; }
    public byte HeaderChecksum { get; private set; }



    
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
        "Broderbund", "Toei Animation", "Toho", "Namco", "Acclaim Entertainment",
        "ASCII Corporation or Nexsoft", "Bandai", "Square Enix", "HAL Laboratory", "SNK",
        "Pony Canyon", "Culture Brain", "Sunsoft", "Sony Imagesoft", "Sammy Corporation",
        "Taito", "Kemco", "Square", "Tokuma Shoten", "Data East",
        "Tonkin House", "Koei", "UFL", "Ultra Games", "VAP, Inc.",
        "Use Corporation", "Meldac", "Pony Canyon", "Angel", "Taito",
        "SOFEL (Software Engineering Lab)", "Quest", "Sigma Enterprises", "ASK Kodansha Co.", "Naxat Soft",
        "Copya System", "Banpresto", "Tomy", "LJN", "Nippon Computer Systems",
        "Human Ent.", "Altron", "Jaleco", "Towa Chiki", "Yutaka",
        "Varie", "Epoch", "Athena", "Asmik Ace Entertainment", "Natsume",
        "King Records", "Atlus", "Epic/Sony Records", "IGS", "A Wave",
        "Extreme Entertainment", "LJN"
    };

    private Dictionary<string, string> NewLicenseeCodes = new Dictionary<string, string> {
        { "00", "None" },
        { "01", "Nintendo Research & Development 1" },
        { "08", "Capcom" },
        { "13", "EA (Electronic Arts)" },
        { "18", "Hudson Soft" },
        { "19", "B-AI" },
        { "20", "KSS" },
        { "22", "Planning Office WADA" },
        { "24", "PCM Complete" },
        { "25", "San-X" },
        { "28", "Kemco" },
        { "29", "SETA Corporation" },
        { "30", "Viacom" },
        { "31", "Nintendo" },
        { "32", "Bandai" },
        { "33", "Ocean Software/Acclaim Entertainment" },
        { "34", "Konami" },
        { "35", "HectorSoft" },
        { "37", "Taito" },
        { "38", "Hudson Soft" },
        { "39", "Banpresto" },
        { "41", "Ubi Soft" },
        { "42", "Atlus" },
        { "44", "Malibu Interactive" },
        { "46", "Angel" },
        { "47", "Bullet-Proof Software" },
        { "49", "Irem" },
        { "50", "Absolute" },
        { "51", "Acclaim Entertainment" },
        { "52", "Activision" },
        { "53", "Sammy USA Corporation" },
        { "54", "Konami" },
        { "55", "Hi Tech Expressions" },
        { "56", "LJN" },
        { "57", "Matchbox" },
        { "58", "Mattel" },
        { "59", "Milton Bradley Company" },
        { "60", "Titus Interactive" },
        { "61", "Virgin Games Ltd." },
        { "64", "Lucasfilm Games" },
        { "67", "Ocean Software" },
        { "69", "EA (Electronic Arts)" },
        { "70", "Infogrames" },
        { "71", "Interplay Entertainment" },
        { "72", "Broderbund" },
        { "73", "Sculptured Software" },
        { "75", "The Sales Curve Limited" },
        { "78", "THQ" },
        { "79", "Accolade" },
        { "80", "Misawa Entertainment" },
        { "83", "lozc" },
        { "86", "Tokuma Shoten" },
        { "87", "Tsukuda Original" },
        { "91", "Chunsoft Co." },
        { "92", "Video System" },
        { "93", "Ocean Software/Acclaim Entertainment" },
        { "95", "Varie" },
        { "96", "Yonezawa/s’pal" },
        { "97", "Kaneko" },
        { "99", "Pack-In-Video" },
        { "9H", "Bottom Up" },
        { "A4", "Konami (Yu-Gi-Oh!)" },
        { "BL", "MTO" },
        { "DK", "Kodansha" }
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


        // Licensee Code
        // Old Licensee Code 是 0x33 => 使用 Old Licensee Code 表
        // 否則使用 New Licensee Code 表
        OldLicenseeCode = romData[(int) Header.OldLicenseeCode];
        if (OldLicenseeCode == 0x33)
        {
            // 使用 New Licensee
            // 將 0x0144 , 0x0145 轉成 ASCII
            string newLicenseeCode = Encoding.ASCII.GetString(romData, (int) Header.NewLicenseeCode1, 2);
            Publisher = NewLicenseeCodes[newLicenseeCode];
        }
        else
        {
            // 使用 Old Licensee
            Publisher = OldLicenseeCodes[OldLicenseeCode];
        }


        // ROM Version ( 這個值通常是 0x00 )
        ROMVersion = romData[(int) Header.ROMVersion];


        // Header Checksum
        HeaderChecksum = romData[(int) Header.HeaderChecksum];
        bool passed = false;
        // Header Checksum 檢查
        int checksum = 0;
        for (int i = 0x0134 ;  i <= 0x014C ; i ++)
        {
            checksum = checksum - romData[i] - 1;
        }
        // 取出 checksum 低八位
        checksum = checksum & 0xFF;

        // 檢驗 checksum
        if ( checksum == HeaderChecksum)
        {
            passed = true;
        }
        else
        {
            passed = false;
        }

        // 輸出卡匣 Header 到終端
        Console.WriteLine($"{"Title", -20} : {Title, -15}");
        Console.WriteLine($"{"Cartridge Type", -20} : {CartridgeTypes[CartridgeType], -15}");
        Console.WriteLine($"{"ROM Size", -20} : {ROMSize + (romData[(int) Header.ROMSize] <= 0x04 ? "KiB" : "MiB"), -15}");
        Console.WriteLine($"{"RAM Size", -20} : {RAMSize + "KiB", -15}");
        Console.WriteLine($"{"Publisher", -20} : {Publisher, -15}");
        Console.WriteLine($"{"ROMVersion", -20} : {ROMVersion, -15}");
        Console.WriteLine($"{"HeaderChecksum", -20} : {HeaderChecksum, -15:X}({(passed ? "通過" : "未通過")})");
    }
}