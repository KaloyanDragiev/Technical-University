namespace Calculator
{
    using System;
    using Functions;
    using System.Text;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    public static class Extensions
    {
        public static int IndexOfNth(this string str, string value, int nth = 1)
        {
            if (nth <= 0)
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            int offset = str.IndexOf(value);
            for (int i = 1; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }
    }
    public class Parser
    {
        public Node ParseExpression(string input)   
        {
            Regex regularExpression = new Regex("^[\\^.,()*/\\-+scel!nrpx0-9 ]*$");

            if (!regularExpression.IsMatch(input)) throw new Exception("The input is not correct!");

            int leftBracketsCount = input.Count(x => x == '(');
            int rightBracketsCount = input.Count(x => x == ')');

            if (leftBracketsCount != rightBracketsCount) throw new Exception("Number of brackets are not equal!");

            input = input.Replace(" ", string.Empty);

            if (input.Length <= 0) return null;

            if (input.Length == 1)
            {
                if (!char.IsNumber(input[0]) && input != "x" && input != "p") throw new InvalidExpressionException("Input is not correct!");
            }
            
            return Parse(ref input);
        }

        private static Node Parse(ref string input)
        {
            Node firstNode;

            char operand;

            if (input[0] == '+' || input[0] == '-' || input[0] == '*' || input[0] == '/' || input[0] == '^')
            {
                operand = input[0];
                input = input.Substring(1);

                SkipCharacter(ref input, '(');
                firstNode = Parse(ref input);
                SkipCharacter(ref input, ',');

                Node secondNode = Parse(ref input);  
                SkipCharacter(ref input, ')');

                if (operand == '+') return new PlusSignFunction(firstNode, secondNode);
                if (operand == '-') return new MinusSignFunction(firstNode, secondNode);
                if (operand == '*') return new MultiplicationSignFunction(firstNode, secondNode);
                if (operand == '/') return new DivisionSignFunction(firstNode, secondNode);

                return new PowerFunction(firstNode, secondNode);
            }
            if (input[0] == 's' || input[0] == 'c' || input[0] == 'e' || input[0] == 'l' || input[0] == '!')
            {
                operand = input[0];

                input = input.Substring(1);
                SkipCharacter(ref input, '(');

                bool isFac = input.Substring(1,1) == ",";

                if (isFac)
                {
                    StringBuilder sb = new StringBuilder(); 
                    sb.Append(input.Substring(0, 1));

                    var counterOpen = input.Count(x => x == '('); 

                    if (counterOpen == 0)
                    {
                        sb.Append(input.Substring(input.IndexOf(')')));
                    }
                    else
                    {
                        var position = Extensions.IndexOfNth(input, ")", counterOpen);

                        sb.Append(input.Substring(position));
                    }

                    input = sb.ToString();
                }

                firstNode = Parse(ref input);
                
                SkipCharacter(ref input, ')');

                if (operand == 's') return new SinFunction(firstNode);
                if (operand == 'c') return new CosFunction(firstNode);
                if (operand == 'e') return new ExponentialFunction(firstNode);
                if (operand == 'l') return new LogarithmFunction(firstNode);

                return new FactorialFunction(firstNode);
            }

            if (input[0] == 'n' || input[0] == 'r')
            {
                operand = input[0];

                input = input.Substring(1);
                SkipCharacter(ref input, '(');

                string temp = input.Split(')')[0];

                if (operand == 'n')
                {
                    if (temp.Contains('.')) throw new Exception("This is not a valid natural number!"); 
                }
                input = input.Substring(input.IndexOf(')') + 1);

                if (operand == 'n') return new NaturalNumberNode(int.Parse(temp));

                return new RationalNumberNode(float.Parse(temp));
            }

            if (input[0] == 'p')
            {
                input = input.Substring(1);
                return new PiNode();
                
                /*if (input.Length == 1)
                {
                    //return new PiNode();
                    return new RationalNumberNode(3.14f);
                }

                if (input.Length == 2)
                {
                    input = input.Substring(1);
                    //return new PiNode();
                    return new RationalNumberNode(3.14f);
                }

                //if (input.Substring(1, 2).Contains("(") || !input.Substring(1, 2).Contains(","))
                //    throw new InvalidExpressionException("The given expression is not valid!");

                input = input.Substring(1);
                //return new PiNode();
                return new RationalNumberNode(3.14f);*/

            }

            if (input[0] == 'x')
            {
                if (input.Length == 1)
                {
                    return new VariableNode();
                }

                if (input.Length == 2)
                {
                    input = input.Substring(1);
                    return new VariableNode();
                }

                if (input.Substring(1, 2).Contains("(") || !input.Substring(1, 2).Contains(","))
                    throw new InvalidExpressionException("The given expression is not valid!");

                input = input.Substring(1);
                return new VariableNode();
            }

            if (char.IsDigit(input[0]))
            {
                string number = input[0].ToString();
                input = input.Substring(1);

                while (input.Length > 0)
                {
                    if (!char.IsDigit(input[0])) break;

                    number += input[0];
                    input = input.Substring(1);
                }

                firstNode = new NaturalNumberNode(int.Parse(number));
                return firstNode;
            }

            throw new InvalidExpressionException("The given expression is not valid!");
        }

        private static void SkipCharacter(ref string s, char c)
        {
            if (s[0] != c)
            {
                switch (c)
                {
                    case '(':
                       throw new Exception("The given expression is not valid!");  
                    case ',':
                        throw new Exception("Separator Expected!"); 
                    case ')':
                        throw new Exception("The given expression is not valid!");
                }
            }

            s = s.Substring(1);
        }
    }
}
