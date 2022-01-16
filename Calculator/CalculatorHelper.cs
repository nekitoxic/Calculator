using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public enum Operation
    {
        Plus,
        Minus,
        Multiple,
        Divide,
        Result,
        Factorial,
        Percent,
        Pow,
        Square,
        Drop,
        Clear,
        Empty
    }

    public struct Expression
    {
        public Operation operation;
        public Double x;
        public Double y;

        public Expression(Double x = 0, Double y = 0, Operation operation = Operation.Empty)
        {
            this.x = x;
            this.y = y;
            this.operation = operation;
        }

        public Double GetResult()
        {
            switch (operation)
            {
                case Operation.Plus: return x + y;
                case Operation.Minus: return x - y;
                case Operation.Multiple: return x * y;
                case Operation.Divide: return (y > 0) ? x / y : x;
                case Operation.Percent: return (x / 100) * y;
            };

            return 0;
        }

        public Int16 GetPriority(Operation operation)
        {
            switch(operation)
            {
                case Operation.Multiple: return 0;
                case Operation.Divide: return 1;
                case Operation.Plus: return 2;
                case Operation.Minus: return 3;
                default: return 4;
            }
        }

        public Expression SetX(Double x)
        {
            this.x = x;

            return this;
        }
    }

    public class CalculatorHelper
    {
        public CalculatorHelper()
        {
        }

        public static Operation GetOperation(string literal)
        {
            switch (literal)
            {
                case "*": return Operation.Multiple;
                case "➗": return Operation.Divide;
                case "+": return Operation.Plus;
                case "-": return Operation.Minus;
                case "%": return Operation.Percent;
                case "fuc": return Operation.Factorial;
                case "pow2": return Operation.Pow;
                case "=": return Operation.Result;
                case "√": return Operation.Square;
                case "C": return Operation.Drop;
                case "CE": return Operation.Clear;
                default: return Operation.Empty;
            };
        }

        public static bool isValidToConcatOperation(Operation operation)
        {
            switch (operation)
            {
                case Operation.Result:
                case Operation.Factorial:
                case Operation.Square:
                case Operation.Pow:
                case Operation.Drop:
                case Operation.Clear:
                    return false;
                default: return true;
            };
        }

        public static bool isEnum(object value)
        {
            switch (value)
            {
                case Operation.Result:
                case Operation.Percent:
                case Operation.Factorial:
                case Operation.Square:
                case Operation.Pow:
                case Operation.Plus:
                case Operation.Minus:
                case Operation.Divide:
                case Operation.Multiple:
                case Operation.Drop:
                case Operation.Clear:
                    return true;
                default: return false;
            };
        }

        public static Double GetResult(ref string[] exp, Operation operation)
        {
            Double calc = SimpleCalculate(Invoke(ref exp));

            switch(operation)
            {
                case Operation.Drop: return 0;
                case Operation.Factorial: return GetFactorial(calc);
                case Operation.Pow: return Math.Pow(calc, 2);
                case Operation.Square: return Math.Sqrt(calc);
                default: return calc;
            }
        }

        private static Double GetFactorial(Double num)
        {
            if (num <= 1 || num > 25)
            {
                return num;
            }

            return num * GetFactorial(num - 1);
        }
        
        private static Double SimpleCalculate(List<object> expression)
        {
            if (expression.Count() < 3)
            {
                return (Double) expression.First();
            }

            List<Expression> exps = new List<Expression>();

            for (int i = 0; i < expression.Count; i++)
            {
                if (isEnum(expression[i]))
                {
                    Double x = Double.Parse(expression[i - 1].ToString());
                    Double y = Double.Parse(expression[i + 1].ToString());

                    exps.Add(new Expression(x, y, (Operation)expression[i]));
                }
            }

            if (0 < exps.Count)
            {
                Double res = exps.First().GetResult();

                foreach (Expression exp in exps.OrderBy(x => x.GetPriority(x.operation)).ToList().Skip(1))
                {
                    res = exp.SetX(res).GetResult();
                }

                return res;
            }

            return 0;
        }

        private static List<object> Invoke(ref string[] exp)
        {
            List<object> res = new List<object>();

            foreach(string el in exp)
            {
                Operation operation = GetOperation(el);

                if (operation == Operation.Empty)
                {
                    res.Add(Double.Parse(el));
                }
                else
                {
                    res.Add(operation);
                }
            }

            if(isEnum(res.Last()))
            {
                res.RemoveAt(res.Count - 1);
            }

            return res;
        }
    }
}