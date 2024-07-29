using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string A = "ABC_~x+\\+";
            Console.WriteLine(HasSpecialChar(A));
        }

        static bool HasSpecialChar(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^[a-zA-Z0-9_*+~]+$");
        }
    }
}
