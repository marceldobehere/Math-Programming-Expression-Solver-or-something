using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Math_expr_ast
{
   
    class ExpressionToken
    {
        public enum ExpressionTokenType
        {
            NONE,
            VAL,
            ADD,
            SUB,
            MUL,
            DIV,
            OP,
            VAR,
            SET,
        }

        public ExpressionTokenType type = ExpressionTokenType.NONE;
        public double value = 0;
        public ExpressionToken left = null, right = null;
        public char op = ' ';
        public string varname = "";

        public ExpressionToken()
        {

        }

        public ExpressionToken(string varname)
        {
            this.varname = varname;
            type = ExpressionTokenType.VAR;
        }

        public ExpressionToken(string varname, ExpressionToken token)
        {
            this.varname = varname;
            left = token;
            type = ExpressionTokenType.SET;
        }

        public ExpressionToken(char op)
        {
            this.op = op;
            type = ExpressionTokenType.OP;
        }

        public ExpressionToken(double value)
        {
            this.value = value;
            type = ExpressionTokenType.VAL;
        }

        public ExpressionToken(ExpressionToken left, ExpressionTokenType type, ExpressionToken right)
        {
            if (type == ExpressionTokenType.VAL || type == ExpressionTokenType.NONE)
                throw new Exception("Incorrect TokenType!");

            this.left = left;
            this.right = right;

            this.type = type;
        }

        public ExpressionToken(ExpressionToken left, char op, ExpressionToken right)
        {
            if (!"+-*/".Contains(op))
                throw new Exception("Incorrect TokenType!");

            this.left = left;
            this.right = right;

            if (op == '+')
                this.type = ExpressionTokenType.ADD;
            else if (op == '-')
                this.type = ExpressionTokenType.SUB;
            else if (op == '*')
                this.type = ExpressionTokenType.MUL;
            else if (op == '/')
                this.type = ExpressionTokenType.DIV;
        }



        public override string ToString()
        {
            if (type == ExpressionTokenType.NONE)
                return $"";

            else if (type == ExpressionTokenType.VAL)
                return value.ToString(CultureInfo.InvariantCulture);
            else if (type == ExpressionTokenType.VAR)
                return $"<VAR: \"{varname}\">";

            else if (type == ExpressionTokenType.OP)
                return $"<OP: {op}>";

            else if (type == ExpressionTokenType.ADD)
                return $"<{left} + {right}>";
            else if (type == ExpressionTokenType.SUB)
                return $"<{left} - {right}>";
            else if (type == ExpressionTokenType.MUL)
                return $"<{left} * {right}>";
            else if (type == ExpressionTokenType.DIV)
                return $"<{left} / {right}>";

            else if (type == ExpressionTokenType.SET)
                return $"<SET \"{varname}\" to {left}>";

            return "<UNDEFINED>";
        }

    }
}
