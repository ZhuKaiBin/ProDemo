using Scalar2Sln_Domain.Common;
using Scalar2Sln_Domain.Exceptions;

namespace Scalar2Sln_Domain.ValueObjects
{
    public class Colour(string code) : ValueObject
    {
        public static Colour From(string code)
        {
            var colour = new Colour(code);

            if (!SupportedColours.Contains(colour))
            {
                throw new UnsupportedColourException(code);
            }

            return colour;
        }


        public static Colour White => new("#FFFFFF");

        public static Colour Red => new("#FF5733");

        public static Colour Orange => new("#FFC300");

        public static Colour Yellow => new("#FFFF66");

        public static Colour Green => new("#CCFF99");

        public static Colour Blue => new("#6666FF");


        public string Code { get; private set; } = string.IsNullOrWhiteSpace(code) ? "#000000" : code;

        public override string ToString()
        {
            return Code;
        }


        //这个是隐式的转换，传入一个colour，然后返回一个string类型的字符串
        
        public static implicit operator string(Colour colour)
        {
            return colour.ToString();
        }

        //这个是显示的类型转换，传入一个string类型，然后转换成一个Colour
        public static explicit operator Colour(string code)
        {
            return From(code);
        }

        //implicit （隐式转换）：
        //不需要显式转换操作符，编译器自动帮你转换，不会丢失信息，一般是安全的转换。
        //explicit （显式转换）：
        //需要你在代码里写转换操作符（强制转换），告诉编译器你知道在做转换，可能会有风险或信息丢失。



        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        protected static IEnumerable<Colour> SupportedColours
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
            }

        }
    }
}
