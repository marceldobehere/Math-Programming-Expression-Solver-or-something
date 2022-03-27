using System;
using System.Globalization;

namespace Math_expr_ast
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter Expression:");
                string expr = Console.ReadLine();
                try
                {
                    Console.WriteLine($"Result: {ExpressionSolver.SolveExpression(expr).ToString(CultureInfo.InvariantCulture)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed To Calculate Result. Error: {e.Message}");
                }
                Console.WriteLine();
            }
        }
    }
}
