using Newtonsoft.Json;

public class Cartridge
{
    [JsonProperty]
    private u8[] _rom;
    [JsonProperty]
    private MBC1 _ICartridgeType;
    [JsonProperty]
    private string _filePath;

    public Cartridge(string filePath)
    {
        this._filePath = filePath;
        LoadCart();
    }

    public Cartridge()
    {
        
    }

    private void LoadCart()
    {
        _rom = File.ReadAllBytes(_filePath);
        
        // Cartridge Type
        if (_rom[0x147] == 0x01) // MBC1
        {
            _ICartridgeType = new MBC1(ref _rom);
        }
    }

    public MBC1 GetMBC()
    {
        return _ICartridgeType;
    }
}