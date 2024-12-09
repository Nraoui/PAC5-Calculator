using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// The main window of the calculator application.
    /// Handles user interactions and communicates with the core logic to perform calculations.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CalculadoraCore calculatorCore;
        /// <summary>
        /// Stores the current mathematical operation as a string.
        /// </summary>
        private string currentOperation = "";
        /// <summary>
        /// List to store the history of operations (for the purpose of navigation through past calculations).
        /// </summary>
        private readonly List<string> operationHistory = new List<string>();
        /// <summary>
        /// The current index for navigating through the operation history.
        /// Starts at -1, meaning no history is selected yet.
        /// </summary>
        private int historyIndex = -1;
        /// <summary>
        /// Flag indicating if the next number entered should be negative.
        /// Used for handling the toggle of the sign of the number.
        /// </summary>
        private bool isNextNumberNegative = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Sets up the UI components and initializes the calculator core logic.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            calculatorCore = new CalculadoraCore(); // Initialize the calculator core logic
        }

        /// <summary>
        /// Handles clicks on number and operator buttons.
        /// Appends the clicked button's content to the current operation string.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            var button = sender as System.Windows.Controls.Button;
            currentOperation += button.Content.ToString();
            Display.Text = currentOperation;
        }

        /// <summary>
        /// Handles clicks on operator buttons (+, -, *, /).
        /// Appends the clicked operator to the current operation string if valid.
        /// </summary>
        /// <param name="sender">The operator button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
           

            var button = sender as System.Windows.Controls.Button;
            if (!string.IsNullOrEmpty(currentOperation) && !IsLastCharOperator())
            {
                currentOperation += button.Content.ToString();
                Display.Text = currentOperation;
            }
        }


        /// <summary>
        /// Handles the click of the decimal button.
        /// Ensures that only one decimal is added to each number in the operation.
        /// </summary>
        /// <param name="sender">The decimal button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            

            if (!IsLastNumberDecimal())
            {
                currentOperation += ".";
                Display.Text = currentOperation;
            }
        }

        /// <summary>
        /// Handles the equals button click.
        /// Evaluates the current mathematical operation and displays the result.
        /// </summary>
        /// <param name="sender">The equals button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsLastCharOperator())
                {
                    Display.Text = "Error";
                    currentOperation = ""; // Reset operation
                    return;
                }

                double result = calculatorCore.EvaluateExpression(currentOperation);

                string operationResult = $"{currentOperation} = {result}";
                operationHistory.Add(operationResult);
                historyIndex = operationHistory.Count; // Reset history navigation

                Display.Text = $"{currentOperation} = {result}";
                currentOperation = result.ToString(); // Update operation
            }
            catch (DivideByZeroException ex)
            {
                Display.Text = ex.Message; // Show error message
                currentOperation = ""; // Reset operation
            }
            catch (FormatException ex)
            {
                Display.Text = ex.Message; // Show error message
                currentOperation = ""; // Reset operation
            }
            catch (InvalidOperationException ex) // Catch the square root error
            {
                Display.Text = ex.Message; // Show error message for square root
                currentOperation = ""; // Reset operation
            }
            catch (Exception)
            {
                Display.Text = "Error";
                currentOperation = "";
            }
        }



        /// <summary>
        /// Clears the current operation and resets the display.
        /// </summary>
        /// <param name="sender">The clear button that was clicked.</param>
        /// <param name="e">Event data.</param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            currentOperation = "";
            Display.Text = "0";
           
        }
        /// <summary>
        /// Handles the click event for the square root button.
        /// Adds the square root operator (represented by √) to the current operation.
        /// </summary>
        /// <param name="sender">The square root button that was clicked.</param>
        /// <param name="e">Event data for the button click.</param>
        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            
            currentOperation += "√";

            Display.Text = currentOperation;
        }

        /// <summary>
        /// Checks if the last character in the current operation is an operator.
        /// </summary>
        /// <returns>True if the last character is an operator (+, -, *, /), otherwise false.</returns>
        private bool IsLastCharOperator()
        {
            if (string.IsNullOrEmpty(currentOperation)) return false;
            char lastChar = currentOperation[^1];
            return lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '/';
        }

        /// <summary>
        /// Checks if the last number in the current operation contains a decimal point.
        /// </summary>
        /// <returns>True if the last number contains a decimal point, otherwise false.</returns>
        private bool IsLastNumberDecimal()
        {
            var parts = currentOperation.Split(new char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string lastNumber = parts[^1];
                return lastNumber.Contains(".");
            }
            return false;
        }
        /// <summary>
        /// Handles the backspace button click event.
        /// Removes the last character from the current operation.
        /// </summary>
        /// <param name="sender">The backspace button that was clicked.</param>
        /// <param name="e">Event data for the button click.</param>
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentOperation))
            {
                currentOperation = currentOperation.Substring(0, currentOperation.Length - 1);
                Display.Text = string.IsNullOrEmpty(currentOperation) ? "0" : currentOperation;
            }
        }

        /// <summary>
        /// Navigates to the previous operation in history (↑ button).
        /// </summary>
        /// <param name="sender">The history up button that was clicked.</param>
        /// <param name="e">Event data for the button click.</param>
        private void HistoryUp_Click(object sender, RoutedEventArgs e)
        {
            if (historyIndex > 0)
            {
                historyIndex--;
                Display.Text = operationHistory[historyIndex];
            }
        }

        /// <summary>
        /// Navigates to the next operation in history (↓ button).
        /// </summary>
        /// <param name="sender">The history down button that was clicked.</param>
        /// <param name="e">Event data for the button click.</param>
        private void HistoryDown_Click(object sender, RoutedEventArgs e)
        {
            if (historyIndex < operationHistory.Count - 1)
            {
                historyIndex++;
                Display.Text = operationHistory[historyIndex];
            }
        }
        /// <summary>
        /// Toggles the sign of the current number (adds a negative sign).
        /// </summary>
        /// <param name="sender">The toggle sign button that was clicked.</param>
        /// <param name="e">Event data for the button click.</param>
        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
           
            currentOperation += "-";  // Immediately show "-" if the input is empty or at the operator section
            Display.Text = currentOperation;
            isNextNumberNegative = true;

        }




    }
}