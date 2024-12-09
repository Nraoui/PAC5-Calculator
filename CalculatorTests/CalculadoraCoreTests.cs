using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculatorTests
{
    public class CalculadoraCoreTests
    {
        private readonly CalculadoraCore _calculadora;

        public CalculadoraCoreTests()
        {
            _calculadora = new CalculadoraCore();
        }

        [Fact]
        public void TestSuma_Success()
        {
            var resultat = _calculadora.EvaluateExpression("5 + 3");
            Assert.Equal(8, resultat);
        }

        [Fact]
        public void TestResta_Success()
        {
            var resultat = _calculadora.EvaluateExpression("5 - 3");
            Assert.Equal(2, resultat);
        }

        [Fact]
        public void TestMultiplicacio_Success()
        {
            var resultat = _calculadora.EvaluateExpression("5 * 3");
            Assert.Equal(15, resultat);
        }

        [Fact]
        public void TestDivisio_Success()
        {
            var resultat = _calculadora.EvaluateExpression("5 / 2");
            Assert.Equal(2.5, resultat);
        }

        [Fact]
        public void TestDivisio_PerZero_ThrowsException()
        {
            Assert.Throws<DivideByZeroException>(() => _calculadora.EvaluateExpression("5 / 0"));
        }

        [Fact]
        public void TestOrderOfOperations()
        {
            var resultat = _calculadora.EvaluateExpression("5 + 3 * 2 - 4 / 2");
            Assert.Equal(9, resultat);
        }

        [Fact]
        public void TestExponentiation_Success()
        {
            var resultat = _calculadora.EvaluateExpression("2 ^ 3");
            Assert.Equal(8, resultat);
        }

        [Fact]
        public void TestExponentiationWithZero_Success()
        {
            var resultat = _calculadora.EvaluateExpression("0 ^ 5");
            Assert.Equal(0, resultat);
        }

        [Fact]
        public void TestSquareRoot_Success()
        {
            var resultat = _calculadora.EvaluateExpression("√16");
            Assert.Equal(4, resultat);
        }

        [Fact]
        public void TestSquareRootWithZero_Success()
        {
            var resultat = _calculadora.EvaluateExpression("√0");
            Assert.Equal(0, resultat);
        }

        [Fact]
        public void TestInvalidSquareRootSyntax_ThrowsException()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("3√6"));
        }

        [Fact]
        public void TestMultiplicationWithSquareRoot_Success()
        {
            var resultat = _calculadora.EvaluateExpression("3*√9");
            Assert.Equal(9, resultat);
        }

        [Fact]
        public void TestSquareRootWithInvalidInput_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _calculadora.EvaluateExpression("√-4"));
        }

        [Fact]
        public void TestParentheses_Success()
        {
            var resultat = _calculadora.EvaluateExpression("(5+3)*2");
            Assert.Equal(16, resultat);
        }

        [Fact]
        public void TestNestedParentheses_Success()
        {
            var resultat = _calculadora.EvaluateExpression("((2+3)*2)");
            Assert.Equal(10, resultat);
        }

        [Fact]
        public void TestInvalidParenthesesFormat_ThrowsException()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("5+(3*2"));
        }

        [Fact]
        public void TestInvalidParenthesesFormat_ThrowsException_2()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("5+3)*2"));
        }

        [Fact]
        public void TestComplexExpression_Success()
        {
            var resultat = _calculadora.EvaluateExpression("3+5*(2^3)-√16");
            Assert.Equal(39, resultat);
        }

        [Fact]
        public void TestInvalidMultiplicationByParentheses_ThrowsException()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("2(8+2)"));
        }

        [Fact]
        public void TestInvalidParenthesesMultiplication_ThrowsException()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("(8+2)2"));
        }

        [Fact]
        public void TestComplexExpressionWithParenthesesAndOperations_Success()
        {
            var resultat = _calculadora.EvaluateExpression("3+5*(2^3)-√16");
            Assert.Equal(39, resultat);
        }

        [Fact]
        public void TestInvalidImplicitMultiplication_ThrowsException()
        {
            Assert.Throws<FormatException>(() => _calculadora.EvaluateExpression("(5)3 + 2"));
        }

        [Fact]
        public void TestExpressionWithExtraSpaces_Success()
        {
            var resultat = _calculadora.EvaluateExpression("5        * 3 + 2");
            Assert.Equal(17, resultat);
        }
    



    /* TODO: Proves pendents a elaborar i afegir

        01. [SUCCESS] 5 + 3 * 2 - 4 / 2 = 9
        02. [SUCCESS] 2 ^ 3 = 8
        03. [SUCCESS] 0 ^ 5 = 0
        04. [SUCCESS] √ 16 = 4
        05. [SUCCESS] √ 0 = 0
        06. [FORMATEXCEPTION] 3 √ 6 = FAIL
        07. [SUCCESS] 3 * √ 9 = 9
        08. [INVALIDOPERATIONEXCEPTION] √ -4 = FAIL
        09. [SUCCESS] ( 5 + 3 ) * 2 = 16
        10. [SUCCESS] ( ( 2 + 3 ) * 2 ) = 10
        11. [FORMATEXCEPTION] 5 + ( 3 * 2 = FAIL
        12. [FORMATEXCEPTION] 5 + 3 ) * 2 = FAIL
        13. [SUCCESS] 3 + 5 * ( 2 ^ 3 ) - √ 16 = 39
        14. [FORMATEXCEPTION] 2(8 + 2) = FAIL
        15. [FORMATEXCEPTION] (8 + 2)2 = FAIL
        16. [SUCCESS] 3 + 5 * ( 2 ^ 3 ) - √ 16 = 39
        17. [FORMATEXCEPTION] (5)3 + 2 = FAIL
        18. [SUCCESS] 5        * 3 + 2 = 17
    */
}




}
