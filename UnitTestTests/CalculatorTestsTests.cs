using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Tests
{
    [TestClass()]
    public class CalculatorTestsTests
    {
        [TestMethod()]
        [DataRow(100, 20)]
        [DataRow(200, 20)]
        [DataRow(200, null)]
        public void TestAddTest(int num1, int num2)
        {

            CalculatorTests calculatorTests = new CalculatorTests();

            var ret = calculatorTests.TestAdd(num1, num2);

            Assert.AreEqual(ret, num1 - num2);
        }
    }
}