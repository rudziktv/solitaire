namespace Utils
{
    public class MathC
    {
        public static int Sum(int up)
        {
            if (up <= 0) return 0;
            return up + Sum(up - 1);
        }
    }
}