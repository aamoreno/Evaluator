using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluator
{
    public class Evaluator
    {
        public enum Operation
        {
            ADDITION,
            SUBSTRACTION,
            MULTIPLICATION,
            DIVISION,
            POWER,
            SQUAREROOT
        }

        /**
         * Evaluates the string and returns an mathematical answer if possible
         **/
        public static double evaluate(String inputString)
        {
            double result = 0;

            if (String.IsNullOrWhiteSpace(inputString))
            {
                return result;
            }

            List<string> expression = split(inputString);
            List<string> updatedExpression = new List<string>();


            /**
             * Precedence is as follows:
             * 
             * 1.) Square root
             * 2.) Exponent
             * 3.) Multiplication
             * 4.) Division
             * 5.) Addition
             * 6.) Subtraction
             **/

            int i = 0;
            foreach (string s in expression)
            {
                if (s.Length > 3)
                {
                    if (s.Substring(0, 4).Equals(getOperationSign(Operation.SQUAREROOT)))
                    {
                        double x = 0;
                        string temp = s.Substring(5);
                        temp = temp.Remove(temp.Length - 1);

                        if (double.TryParse(temp, out x))
                        { 
                            double answer = squareRoot(x);
                            updatedExpression.Add(answer.ToString());
                        }
                    }
                } 
                else
                {
                    updatedExpression.Add(s);
                }
            }

            expression = updatedExpression;

            while (hasOperation(Operation.POWER, expression))
            {
                calculate(Operation.POWER, expression);
            }

            while (hasOperation(Operation.MULTIPLICATION, expression))
            {
                calculate(Operation.MULTIPLICATION, expression);
            }

            while (hasOperation(Operation.DIVISION, expression))
            {
                calculate(Operation.DIVISION, expression);
            }

            while (hasOperation(Operation.ADDITION, expression))
            {
                calculate(Operation.ADDITION, expression);
            }

            while (hasOperation(Operation.SUBSTRACTION, expression))
            {
                calculate(Operation.SUBSTRACTION, expression);
            }

            if (expression.Count == 1)
            {
                double.TryParse(expression[0], out result);
                return result;
            }
            else
            {
                throw new InvalidFormat("Invalid Format!");
            }
        }

        /**
         * Returns a string representation of the operation
         **/
        public static string getOperationSign(Operation operation)
        {
            switch (operation)
            {
                case Operation.ADDITION:
                    return "+";
                case Operation.SUBSTRACTION:
                    return "-";
                case Operation.MULTIPLICATION:
                    return "*";
                case Operation.DIVISION:
                    return "/";
                case Operation.POWER:
                    return "^";
                case Operation.SQUAREROOT:
                    return "sqrt";
                default:
                    return "";
            }
        }

        /**
         * Returns true if the operation is present within the expression.
         * Returns false otherwise.
         **/
        public static bool hasOperation(Operation operation, List<string> expression)
        {
            // Checks if the occurence of the operation is within the expression.
            // 
            // The checking of square root operation is excluded because in the split function,
            // The somewhat regex of it is sqrt(1~9), thus cannot be handled easily by the Contains function

            if (expression.Contains(getOperationSign(operation)))
            {
                return true;
            }
            else
            {
                foreach (string s in expression)
                {
                    if (s.Length > 3)
                    {
                        if (s.Substring(0, 4).Equals(getOperationSign(Operation.SQUAREROOT)))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /**
         * Calculates the expression based on the operations except the square root functionality.
         * */
        public static void calculate(Operation operation, List<string> expression)
        {
            int i = 0;
            while (i < expression.Count)
            {
                if (expression[i].Equals(getOperationSign(operation)))
                {
                    double x = 0, y = 0;
                    if (double.TryParse(expression[i - 1], out x) && double.TryParse(expression[i + 1], out y))
                    {
                        double answer = 0; 

                        switch (operation)
                        {
                            case Operation.ADDITION:
                                answer = add(x, y);
                                break;
                            case Operation.SUBSTRACTION:
                                answer = subtract(x, y);
                                break;
                            case Operation.MULTIPLICATION:
                                answer = multiply(x, y);
                                break;
                            case Operation.DIVISION:
                                answer = divide(x, y);
                                break;
                            case Operation.POWER:
                                answer = raiseToExponent(x, y);
                                break;
                        }

                        // Remove the evaluated expression and replace it with the answer
                        expression.RemoveAt(i - 1);
                        expression.RemoveAt(i - 1);
                        expression.RemoveAt(i - 1);
                        expression.Insert(i - 1, answer.ToString());
                        i += 2;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        /**
         * Splits the input string into a list.
         **/
        public static List<string> split(string inputString)
        {
            List<string> result = new List<string>();

            char[] charArray = inputString.ToCharArray();
            char[] operators = { '+', '-', '*', '/', '^' };
            
            int i = 0;
            while (i < charArray.Length) 
            {
                char c = charArray[i];

                if (Char.IsDigit(c))
                {
                    string digit = "" + c;
                    i += 1;
                    while (i < charArray.Length)
                    {
                        if (Char.IsDigit(charArray[i]))
                        {
                            digit += charArray[i];
                            ++i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    result.Add(digit);
                }
                else if (operators.Contains(c))
                {
                    result.Add(c.ToString());
                    ++i;
                }
                else if (c.Equals('s'))
                {
                    if ((charArray.Length - (i + 1)) > 5)// smallest possible size is 7 characters : sqrt(x)
                    {
                        string s = c + "" + charArray[i + 1] + "" + charArray[i + 2] + "" + charArray[i + 3] + "" + charArray[i + 4]; // s q r t (
                        i += 5;

                        if (s.Equals("sqrt("))
                        {
                            if (Char.IsDigit(charArray[i]))
                            {
                                string paramValue = "" + charArray[i];
                                i += 1;
                                while (i < charArray.Length)
                                {
                                    if (Char.IsDigit(charArray[i]))
                                    {
                                        paramValue += charArray[i];
                                        ++i;
                                    }
                                    else
                                    {
                                        if (paramValue.Length > 0 && charArray[i].Equals(')'))
                                        {
                                            // By this point s should be "sqrt(x)"
                                            s += paramValue + "" + charArray[i].ToString();
                                            result.Add(s);
                                            ++i;
                                        }
                                        else
                                        {
                                            throw new InvalidFormat("Invalid Format!");
                                        }

                                        break;
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidFormat("Invalid Format!");
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidFormat("Invalid Format!");
                    }

                    
                }
                else
                {
                    throw new InvalidFormat("Invalid Format!");
                }
            }

            return result;
        }

        public static double add(double x, double y)
        {
            return x + y;
        }

        public static double subtract(double x, double y)
        {
            return x - y;
        }

        public static double multiply(double x, double y)
        {
            return x * y;
        }

        public static double divide(double x, double y) 
        {
            if (y == 0)
            {
                throw new DivideByZeroException();
            }
            return x / y;
        }

        public static double raiseToExponent(double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double squareRoot(double x)
        {
            return Math.Sqrt(x);
        }

    }
}
