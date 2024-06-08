namespace ProDecmialCompareToEqual
{
    public class Program
    {
        private static void Main(string[] args)
        {
            double d1 = 1.0;
            double d2 = 1.00;

            var equalResult = d1.Equals(d2);

            var compareResult = d1.CompareTo(d2);
        }
    }
}