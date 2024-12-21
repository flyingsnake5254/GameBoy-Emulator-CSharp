public class Record
{
    public CPU Cpu { get; set; }
    public MMU Mmu { get; set; }
    public PPU Ppu { get; set; }
    public Timer Timer { get; set; }
    public Cartridge Cartridge { get; set; }
    public MBC1 Mbc { get; set; }
    public DateTime Timestamp { get; set; }

    public int Cycles { get; set; }
    public bool Running { get; set; }

    public Record(CPU cpu, MMU mmu, PPU ppu, Timer timer, Cartridge cartridge, MBC1 mbc, int cycles, bool running)
    {
        Cpu = cpu;
        Mmu = mmu;
        Ppu = ppu;
        Timer = timer;
        Cycles = cycles;
        Cartridge = cartridge;
        Mbc = mbc;
        Running = running;
        Timestamp = DateTime.Now;
    }
}
