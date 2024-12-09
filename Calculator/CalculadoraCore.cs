using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    /// <summary>
    /// Core class responsible for evaluating mathematical expressions.
    /// Provides support for basic arithmetic operations and square root calculations.
    /// </summary>
    public class CalculadoraCore
    {
        /// <summary>
        /// Evaluates a mathematical expression and returns the result as a string.
        /// </summary>
        /// <param name="expression">The mathematical expression to evaluate.</param>
        /// <returns>The result of the evaluated expression.</returns>
        public double EvaluateExpression(string expression)
        {
            try
            {
                // Validate parentheses before processing
                if (!AreParenthesesBalanced(expression))
                    throw new FormatException("Unbalanced ()");
                if (!AreOperatorsValid(expression))
                    throw new FormatException("Missing operator");


                return Evaluate(expression);
            }
            catch (DivideByZeroException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Recursively evaluates expressions, handling parentheses and operator precedence.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated result.</returns>
        private double Evaluate(string expression)
        {
            expression = expression.Replace(" ", ""); // Remove spaces

            // Handle parentheses first
            while (expression.Contains("("))
            {
                int openIndex = expression.LastIndexOf('(');
                int closeIndex = expression.IndexOf(')', openIndex);

                if (closeIndex == -1)
                    throw new FormatException("Unbalanced ()");

                string innerExpression = expression.Substring(openIndex + 1, closeIndex - openIndex - 1);
                double innerResult = Evaluate(innerExpression);
                expression = expression.Substring(0, openIndex) + innerResult.ToString() + expression.Substring(closeIndex + 1);
            }

            // Parse and evaluate the flat expression (no parentheses left)
            return EvaluateFlatExpression(expression);
        }

        /// <summary>
        /// Evaluates a flat expression without parentheses.
        /// Supports operators: +, -, *, /, ^, and √.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated result.</returns>
        private double EvaluateFlatExpression(string expression)
        {
            var numbers = new List<double>();
            var operators = new List<char>();

            int i = 0;
            while (i < expression.Length)
            {
                char currentChar = expression[i];

                if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    string number = "";
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }
                    numbers.Add(double.Parse(number));
                }
                else if ("+-*/^".Contains(currentChar))
                {
                    operators.Add(currentChar);
                    i++;
                }
                else if (currentChar == '√')
                {

                    i++;
                    string number = "";

                    // Handle optional negative number after ✓
                    if (i < expression.Length && expression[i] == '-')
                    {
                        number += "-";
                        i++;
                    }

                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }

                    if (string.IsNullOrEmpty(number))
                    {
                        if (numbers.Count > 0)
                        {
                            double value = numbers.Last();
                            numbers[numbers.Count - 1] = Math.Sqrt(value);
                        }
                        else
                        {
                            throw new ArgumentException("Square root requires a number.");
                        }
                    }
                    else
                    {
                        // If no valid number follows the square root operator or if there's an invalid format
                        if (string.IsNullOrEmpty(number))
                        {
                            throw new FormatException("Invalid sqrt"); // Invalid sqrt case
                        }
                        double value = double.Parse(number);
                        if (value < 0)
                            throw new InvalidOperationException("Invalid sqrt");
                        numbers.Add(Math.Sqrt(value));
                    }
                }
                else
                {
                    throw new FormatException("Invalid character in expression.");
                }
            }

            // Process exponentiation first (right-associative)
            for (int j = operators.Count - 1; j >= 0; j--)
            {
                if (operators[j] == '^')
                {
                    double baseNum = numbers[j];
                    double exponent = numbers[j + 1];
                    numbers[j] = Math.Pow(baseNum, exponent);
                    numbers.RemoveAt(j + 1);
                    operators.RemoveAt(j);
                }
            }

            // Process multiplication and division
            for (int j = 0; j < operators.Count; j++)
            {
                if (operators[j] == '*' || operators[j] == '/')
                {
                    double left = numbers[j];
                    double right = numbers[j + 1];

                    numbers[j] = operators[j] == '*' ? Multiply(left, right) : Divide(left, right);
                    numbers.RemoveAt(j + 1);
                    operators.RemoveAt(j);
                    j--;
                }
            }

            // Process addition and subtraction
            double result = numbers[0];
            for (int j = 0; j < operators.Count; j++)
            {
                result = operators[j] == '+' ? Add(result, numbers[j + 1]) : Subtract(result, numbers[j + 1]);
            }

            return result;
        }

        /// <summary>
        /// Validates if the parentheses in the expression are balanced.
        /// </summary>
        /// <param name="expression">The expression to validate.</param>
        /// <returns>True if parentheses are balanced, false otherwise.</returns>
        private bool AreParenthesesBalanced(string expression)
        {
            int balance = 0;
            foreach (char c in expression)
            {
                if (c == '(') balance++;
                else if (c == ')') balance--;
                if (balance < 0) return false;
            }
            return balance == 0;
        }

        /// <summary>
        /// Validates operator usage in the expression.
        /// </summary>
        /// <param name="expression">The expression to validate.</param>
        /// <returns>True if operators are used correctly, false otherwise.</returns>
        private bool AreOperatorsValid(string expression)
        {
            for (int i = 0; i < expression.Length - 1; i++)
            {
                char current = expression[i];
                char next = expression[i + 1];

                // Detect patterns like ")2" or "(8"
                if ((current == ')' && char.IsDigit(next)) || (char.IsDigit(current) && next == '('))
                {
                    return false; // Missing operator
                }
                // Check for invalid square root usage (e.g., '3 √ 6' is invalid)
                if (char.IsDigit(current) && next=='√')
                {
                    return false; // Valid usage of '√' (at the beginning or after an operator or another √)
                }
                
            }
            return true;
        }



        /// <summary>
        /// Adds two numbers together.
        /// </summary>
        /// <param name="a">The first number to add.</param>
        /// <param name="b">The second number to add.</param>
        /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/>.</returns>

        private double Add(double a, double b) => a + b;
        /// <summary>
        /// Subtracts the second number from the first.
        /// </summary>
        /// <param name="a">The first number to subtract from.</param>
        /// <param name="b">The number to subtract.</param>
        /// <returns>The result of <paramref name="a"/> - <paramref name="b"/>.</returns>
        private double Subtract(double a, double b) => a - b;
        /// <summary>
        /// Multiplies two numbers.
        /// </summary>
        /// <param name="a">The first number to multiply.</param>
        /// <param name="b">The second number to multiply.</param>
        /// <returns>The product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        private double Multiply(double a, double b) => a * b;
        /// <summary>
        /// Divides the first number by the second.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The quotient of <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
        private double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero");
            return a / b;
        }


    }
}
