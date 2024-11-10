using System.Runtime.InteropServices; //SDL2

public class Emulator
{
    /*
        SDL2
    */
    [DllImport("libSDL2-2.0.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SDL_Init(uint flags);

    [DllImport("libSDL2_ttf-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TTF_Init();


    /*
        Run() 回傳值
    */
    private enum RunResultCode
    {
        ArgsFormatWrong = -1,
        LoadCartridgeFailed = -2,
        SDLInitFailed = -3,
        TTFInitFailed = -4,
        CPUStalls = -5,
    } 


    /*
        GameBoy Components
    */
    private CPU _cpu = new CPU();


    /*
        Emulator State
    */
    private bool _running = true;


    public int Run(string[] args)
    {
        // 參數格式檢查
        if (args.Length < 1)
        {
            Console.WriteLine("用法：dotnet run file.gb");
            return (int) RunResultCode.ArgsFormatWrong;
        }
        Console.WriteLine("V 參數輸入完成");


        // 載入卡匣
        if (Cartridge.Load(args[0]) == false)
        {
            Console.WriteLine("X 卡匣讀取失敗");
            return (int) RunResultCode.LoadCartridgeFailed;
        }
        Console.WriteLine("V 卡匣載入成功");


        // 初始化 SDL
        if (SDL_Init(0x00000020) != 0)  // SDL_INIT_VIDEO flag
        {
            Console.WriteLine("X SDL 初始化失敗");
            return (int) RunResultCode.SDLInitFailed;
        }
        Console.WriteLine("V SDL 初始化成功");

        // 初始化 TTF
        if (TTF_Init() != 0)
        {
            Console.WriteLine("X TTF 初始化失敗");
            return (int) RunResultCode.TTFInitFailed;
        }
        Console.WriteLine("V TTF 初始化成功");


        // 初始化 CPU
        _cpu.Init();


        // 運行 Emulator
        while (_running)
        {
            if (_cpu.Step() == false)
            {
                Console.WriteLine("X CPU 停止運行");
                return (int) RunResultCode.CPUStalls;
            }
        }


        return 0;
    }


    public static void EmulatorCycles(int cpuCycles)
    {
        // TODO...
    }
}