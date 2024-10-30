using System.Runtime.InteropServices; // SDL2
public class Emulator
{
    // SDL2
    [DllImport("libSDL2-2.0.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_Init(uint flags);

    [DllImport("libSDL2_ttf-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TTF_Init();


    // GameBoy Components 
    private Cartridge cartridge = new Cartridge(); // 卡匣
    private CPU cpu = new CPU(); // CPU


    // 模擬器狀態
    private bool running = true;

    public int Run(string[] args)
    {
        // 檢查參數輸入格式
        if (args.Length < 1)
        {
            Console.WriteLine("用法：dotnet run /path/to/file.gb");
            return -1;
        }

        // 讀取卡匣
        if (cartridge.Load(args[0]) == false)
        {
            Console.WriteLine("讀取卡匣失敗");
            return -2;
        }
        Console.WriteLine("讀取卡匣成功");

        // 初始化 SDL
        if (SDL_Init(0x00000020) != 0)  // SDL_INIT_VIDEO flag
        {
            Console.WriteLine("SDL 初始化失敗");
            return -3;
        }
        Console.WriteLine("SDL 初始化成功");

        // 初始化 TTF
        if (TTF_Init() != 0)
        {
            Console.WriteLine("TTF 初始化失敗");
            return -4;
        }
        Console.WriteLine("TTF 初始化成功");

        // 初始化 CPU
        cpu.Init();

        // 運行模擬器
        while (running)
        {
            if (cpu.Step() == false)
            {
                Console.WriteLine("CPU 停止運行");
                return -5;
            }
        }


        return 0;
    }
}