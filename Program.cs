public class Program 
{
    static void Main(string[] args)
    {
        Emulator emulator = new Emulator();
        int result = emulator.Run(args);
        Environment.Exit(result);
    }
}