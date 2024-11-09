using u8 = System.Byte;

public class Utils
{
    public static int GetBit (u8 n, u8 index)
    {
        return (n >> index) & 1;
    }

    public static u8 SetBit(u8 n, u8 index, u8 value)
    {
        if (value == 1)
        {
            // 若要設置為 1，使用 OR 運算符將指定位置的位設為 1
            return (u8)(n | (1 << index));
        }
        else
        {
            // 若要設置為 0，使用 AND 和 NOT 運算符將指定位置的位設為 0
            return (u8)(n & ~(1 << index));
        }
    }

}
