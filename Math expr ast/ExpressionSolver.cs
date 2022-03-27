using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Math_expr_ast
{
    partial class ExpressionSolver
    {

        public static double SolveExpression(string expr)
        {
            ExpressionToken token = ParseExpression(expr);
            if (token == null)
                return 0;

            Console.WriteLine($" - Solving Token: {token}");


            return SolveToken(token);

            //return token.ToString();
        }

        public static double SolveToken(ExpressionToken token)
        {
            if (token.type == ExpressionToken.ExpressionTokenType.VAL)
                return token.value;

            if (token.type == ExpressionToken.ExpressionTokenType.VAR)
                return 0;

            if (token.type == ExpressionToken.ExpressionTokenType.NONE)
                return 0;

            if (token.type == ExpressionToken.ExpressionTokenType.SET)
                return 0;

            if (token.type == ExpressionToken.ExpressionTokenType.OP)
                throw new Exception("Operation Token itself can't be solved");

            if (token.type == ExpressionToken.ExpressionTokenType.ADD)
                return SolveToken(token.left) + SolveToken(token.right);
            if (token.type == ExpressionToken.ExpressionTokenType.SUB)
                return SolveToken(token.left) - SolveToken(token.right);
            if (token.type == ExpressionToken.ExpressionTokenType.MUL)
                return SolveToken(token.left) * SolveToken(token.right);
            if (token.type == ExpressionToken.ExpressionTokenType.DIV)
                return SolveToken(token.left) / SolveToken(token.right);


            throw new Exception("Token could'nt be solved for some reason.");
        }





        public static string numstring = "0123456789.";
        public static string varstring = "abcdefghijklmnopqrstuvwxyz0123456789_";
        public static string opstring = "+-/*=";

        public static ExpressionToken ParseExpression(string expr)
        {
            List<ExpressionToken> tokens = new List<ExpressionToken>();

            for (int i = 0; i < expr.Length; i++)
            {
                if (opstring.Contains(expr[i]))
                {
                    tokens.Add(new ExpressionToken(expr[i]));
                }
                else if (numstring.Contains(expr[i]))
                {
                    string temp = expr[i].ToString();
                    i++;
                    if (i < expr.Length)
                        while (numstring.Contains(expr[i]))
                        {
                            if (expr[i] == '.')
                                if (temp.Contains('.'))
                                    throw new Exception("Invalid Number used as input");
                            
                            temp += expr[i];
                            i++;

                            if (i >= expr.Length)
                                break;
                        }

                    i--;
                    tokens.Add(new ExpressionToken(double.Parse(temp, CultureInfo.InvariantCulture)));
                }
                else if (expr[i] == '(')
                {
                    string temp = "";

                    int level = 1;
                    i++;
                    if (i < expr.Length)
                        while (i < expr.Length)
                        {
                            if (expr[i] == '(')
                                level++;
                            if (expr[i] == ')')
                            {
                                level--;
                                if (level == 0)
                                    break;
                            }

                            temp += expr[i];
                            i++;
                        }
                    i--;

                    tokens.Add(ParseExpression(temp));
                }
                else if (varstring.Contains(Char.ToLower(expr[i])))
                {
                    string temp = expr[i].ToString();
                    i++;
                    if (i < expr.Length)
                        while (varstring.Contains(Char.ToLower(expr[i])))
                        {

                            temp += expr[i];
                            i++;

                            if (i >= expr.Length)
                                break;
                        }

                    i--;
                    tokens.Add(new ExpressionToken(temp));
                }
            }


            while (tokens.Count > 1)
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].type == ExpressionToken.ExpressionTokenType.OP && "*/".Contains(tokens[i].op))
                    {
                        if (i < 1 || i >= tokens.Count - 1)
                            throw new Exception("Operation doesn't have enough values!");

                        tokens[i - 1] = new ExpressionToken(tokens[i - 1], tokens[i].op, tokens[i + 1]);
                        tokens.RemoveRange(i, 2);
                        i--;
                    }
                }

                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].type == ExpressionToken.ExpressionTokenType.OP && tokens[i].op == '+')
                    {
                        if (i < 1 || i >= tokens.Count - 1)
                            throw new Exception("Operation doesn't have enough values!");

                        tokens[i - 1] = new ExpressionToken(tokens[i - 1], tokens[i].op, tokens[i + 1]);
                        tokens.RemoveRange(i, 2);
                        i--;
                    }
                    else if (tokens[i].type == ExpressionToken.ExpressionTokenType.OP && tokens[i].op == '-')
                    {
                        if (i < 1 || i >= tokens.Count - 1)
                        {
                            if (i < 1 && i < tokens.Count - 1)
                            {
                                if (tokens[i - 1].type != ExpressionToken.ExpressionTokenType.VAL)
                                    throw new Exception($"Negative Operator only supports values!");

                                tokens[i - 1] = new ExpressionToken(-tokens[i - 1].value);
                                tokens.RemoveAt(i);
                                i--;
                            }
                            else
                                throw new Exception("Operation doesn't have enough values!");
                        }
                        else
                        {
                            tokens[i - 1] = new ExpressionToken(tokens[i - 1], tokens[i].op, tokens[i + 1]);
                            tokens.RemoveRange(i, 2);
                            i--;
                        }
                    }
                }
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].type == ExpressionToken.ExpressionTokenType.OP && tokens[i].op == '=')
                    {
                        if (i < 1 || i >= tokens.Count - 1)
                            throw new Exception("Operation doesn't have enough values!");

                        if (tokens[i-1].type != ExpressionToken.ExpressionTokenType.VAR)
                            throw new Exception("Can't set value!");

                        tokens[i - 1] = new ExpressionToken(tokens[i - 1].varname, tokens[i + 1]);
                        tokens.RemoveRange(i, 2);
                        i--;
                    }
                }
            }

            //Console.WriteLine($"TOKENS:");
            //for (int i = 0; i < tokens.Count; i++)
            //{
            //    Console.WriteLine($" - {tokens[i]}");
            //}
            //Console.WriteLine();

            if (tokens.Count == 0)
                return new ExpressionToken();

            return tokens[0];
        }





    }
}
