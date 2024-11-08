using u8 = System.Byte;
using u16 = System.Int16;
using u32 = System.Int32;
using System.Text;


public class Cartridge
{
    public struct Header
    {
        public string Title = "";
        public u8 CGBFlag;
        public string ManufacturerCode;
        public u8 CartridgeType;
        public u8 ROMSizeValue;
        public u32 ROMSize;
        public u8 RAMSizeValue;
        public u32 RAMSize;
        public u8 OldLicenseCode;
        public string NewLicenseCode;
        public string Publisher;
        public u8 ROMVersion;
        public u8 HeaderCheckSum;
        public bool HeaderCheckPass;

        // 卡匣 Type 對照表
        private string[] _cartridgeTypes = {
            "ROM ONLY", "MBC1", "MBC1+RAM", "MBC1+RAM+BATTERY", "MBC2",
            "MBC2+BATTERY", "ROM+RAM", "ROM+RAM+BATTERY", "MMM01", "MMM01+RAM",
            "MMM01+RAM+BATTERY", "MBC3+TIMER+BATTERY", "MBC3+TIMER+RAM+BATTERY", "MBC3", "MBC3+RAM",
            "MBC3+RAM+BATTERY", "MBC5", "MBC5+RAM", "MBC5+RAM+BATTERY", "MBC5+RUMBLE",
            "MBC5+RUMBLE+RAM", "MBC5+RUMBLE+RAM+BATTERY", "MBC6", "MBC7+SENSOR+RUMBLE+RAM+BATTERY", "POCKET CAMERA",
            "BANDAI TAMA5", "HuC3", "HuC1+RAM+BATTERY"
        };

        // RAM size 對照表
        private int[] RAMSizes = { 0, -1, 8, 32, 128, 64 };


        // Old License Code Publisher 對照表
        public static string[] OldLicenseeCodes = {
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


        // New License Code Publisher 對照表
        public static Dictionary<string, string> NewLicenseeCodes = new Dictionary<string, string> {
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
        
        
        public Header(
            string title, u8 cgbFlag, string manufacturerCode, u8 cartridgeType, u8 romSizeValue,
            u8 ramSizeValue, u8 oldLicenseCode, string newLicenseCode, string publisher, u8 romVersion,
            u8 headerCheckSum, bool headerCheckPass)
        {
            Title = title;
            CGBFlag = cgbFlag;
            ManufacturerCode = manufacturerCode;
            CartridgeType = cartridgeType;
            ROMSizeValue = romSizeValue;
            ROMSize = 32 * (1 << romSizeValue) * 1024; // ROM Size = 32 KiB × (1 << <value>)
            RAMSizeValue = ramSizeValue;
            RAMSize = RAMSizes[RAMSizeValue] * 1024;
            OldLicenseCode = oldLicenseCode;
            NewLicenseCode = newLicenseCode;
            Publisher = publisher;
            ROMVersion = romSizeValue;
            HeaderCheckSum = headerCheckSum;
            HeaderCheckPass = headerCheckPass;
        }


        public void Output()
        {
            const int space = -20;
            Console.WriteLine($"{"Title", space} | {Title, space}");
            Console.WriteLine($"{"CGB Flag", space} | {CGBFlag, space}");
            Console.WriteLine($"{"Manufacturer Code", space} | {ManufacturerCode, space}");
            Console.WriteLine($"{"Cartridge Type", space} | {_cartridgeTypes[CartridgeType], space}");
            Console.WriteLine($"{"ROM Size Value", space} | {ROMSizeValue, space}");
            Console.WriteLine($"{"ROM Size", space} | {ROMSize} bytes");
            Console.WriteLine($"{"RAM Size Value", space} | {RAMSizeValue} bytes");
            Console.WriteLine($"{"RAM Size", space} | {RAMSize} bytes");
            Console.WriteLine($"{"Old License Code", space} | {OldLicenseCode, space}");
            Console.WriteLine($"{"New License Code", space} | {NewLicenseCode, space}");
            Console.WriteLine($"{"Publisher", space} | {Publisher, space}");
            Console.WriteLine($"{"ROM Version", space} | {ROMVersion, space}");
            Console.WriteLine($"{"Header Check Sum", space} | {HeaderCheckSum, space} {(HeaderCheckPass ? "PASSED" : "(FAILED)")}");
        }
    }

    private static u8[] _romData;
    private static Header _header;


    public static bool Load(string path)
    {
        try
        {
            _romData = File.ReadAllBytes(path);
            Console.WriteLine($"卡匣 ROM Size : {_romData.Length} bytes");

            ParseHeader(_romData, ref _header);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("X 讀取卡匣錯誤：" + ex.ToString());
            return false;
        }
    }


    private static void ParseHeader(u8[] romData, ref Header header)
    {

        u8 cgbFlag = romData[0x0143];
        string manufacturerCode = "";
        string title = "";
        u8 cartridgeType = 0;
        u8 romSizeValue = 0;
        u8 ramSizeValue = 0;
        u8 oldLicenseCode = 0;
        string newLicenseCode = "";
        string publisher = "";
        u8 romVersion = 0;
        u8 headerCheckSum = 0;
        bool headerCheckPass = false;

        /*
        Title, CGB Flag, Manufacturer Code
            CGB Flag (0x0143) 的值為 0x80 or 0xC0
            |__ 否 -->  Title : 0x0134 ~ 0x0142 (15 bytes)
            |__ 是 -->  Title : 0x0134 ~ 0x013E (11 bytes)
                        Manufacturer : 0x013F ~ 0x0142 (4 bytes)
        */
        if (cgbFlag == 0x80 || cgbFlag == 0xC0)
        {
            manufacturerCode = Encoding.ASCII.GetString(romData, 0x013F, 4);
            title = Encoding.ASCII.GetString(romData, 0x0134, 11);
        }
        else
        {
            title = Encoding.ASCII.GetString(romData, 0x0134, 15);
        }


        /*
        Cartridge Type
        */
        cartridgeType = romData[0x0147];


        /*
        ROM Size Value
        */
        romSizeValue = romData[0x0148];


        /*
        RAM Size Value
        */
        ramSizeValue = romData[0x0149];


        /*
        License Code
            Old License Code (0x014B) 的值是 0x33
                |__ 否 --> 將 (0x014b) 對照 Old License Code 表，取得 Publisher
                |__ 是 --> 將 (0x0144 ~ 0x0145) 轉 ASCII 後，對照 New License Code 表，取得 Publisher
        */
        oldLicenseCode = romData[0x014B];
        if (oldLicenseCode == 0x33)
        {
            newLicenseCode = Encoding.ASCII.GetString(romData, 0x0144, 2);
            publisher = Header.NewLicenseeCodes[newLicenseCode];
        }
        else
        {
            publisher = Header.OldLicenseeCodes[oldLicenseCode];
        }


        /*
        ROM Version (0x014C)
        */
        romVersion = romData[0x014C];


        /*
        Header Check Sum (0x014D)
            驗證方式：
                uint8_t checksum = 0;
                for (uint16_t address = 0x0134; address <= 0x014C; address++) {
                    checksum = checksum - rom[address] - 1;
                }

                若 checksum (checksum & 0xFF) == HeaderCheckSum(0x014D) --> Pass
        */
        headerCheckSum = romData[0x014D];
        int checkSum = 0;
        for (u16 i = 0x0134 ; i <= 0x014C ; i ++)
        {
            checkSum = checkSum - romData[i] - 1;
        }
        headerCheckPass = headerCheckSum == (checkSum & 0xFF);



        // 初始化 header
        header = new Header(
            title, cgbFlag, manufacturerCode, cartridgeType, romSizeValue,
            ramSizeValue, oldLicenseCode, newLicenseCode, publisher, romVersion,
            headerCheckSum, headerCheckPass);

        header.Output();
    }
}