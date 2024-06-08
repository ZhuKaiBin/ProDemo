

namespace UnitTest
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }


    public class CalculatorTests
    {
       
        public int TestAdd(int a,int b)
        {
            // Arrange
            Calculator calculator = new Calculator();

            // Act
            int result = calculator.Add(a, b);

            return result;
        }
    }

}
