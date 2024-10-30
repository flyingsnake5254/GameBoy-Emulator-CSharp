/*
卡匣 Header 對應 Index
*/

namespace Global
{
    namespace GCartridge
    {
        public enum Header
        {
            TitleStart = 0x0134,
            TitleEnd15 = 0x0142,
            TitleEnd11 = 0x013E,
            CGBFlag = 0x0143,
            CartridgeType = 0x0147,
            ROMSize = 0x0148,
            RAMSize = 0x0149,
            OldLicenseeCode = 0x014B,
            NewLicenseeCode1 = 0x0144,
            NewLicenseeCode2 = 0x0145,
            ROMVersion = 0x014C,
            HeaderChecksum = 0x014D
        }
    }
    
}