using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!\n");

            List<string> testCases = new List<string> { 
                                        null,
                                        "",
                                        " ",
                                        "aaa",
                                        "1+1a",
                                        "1+2+3",
                                        "2-2+3",
                                        "2*4",
                                        "sqrt(4)",
                                        "2^2",
                                        "2^2+3*6",
                                        "2+4/sqrt(4)-4",
                                        "sqrt(4)+sqrt(4)"
            };

            /**
             * Precedence is as follows:
             * 
             * 1.) Square root
             * 2.) Exponent
             * 3.) Multiplication
             * 4.) Division
             * 5.) Addition
             * 6.) Subtraction
             * 
             * NOTE: This program cannot handle spaces within a math expression.
             *       This program cannot handle expression with open and closed parentheses.
             **/

            Console.WriteLine(Evaluator.evaluate("2-2+3"));

            foreach (string s in testCases)
            {
                try
                {
                    Console.WriteLine("The expression: " + s); 
                    Console.WriteLine("The answer is " + Evaluator.evaluate(s) + "\n");
                }
                catch (InvalidFormat e)
                {
                    Console.WriteLine(e.Message + "\n");
                }
            }
        }
    }
}
