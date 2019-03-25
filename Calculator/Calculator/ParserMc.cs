using System;
using Calculator.Functions;

namespace Calculator
{
    static class ParserMc
    {
        static private string operants = "+-*/^scle!'nrx";
                                                          
        static public Node getFunction(string s)
        {
            if (s == "")
            {
                return null;
            }
            Node f = null;
            int i = 0;
            while (f == null && i <= operants.Length - 1)
            {
                f = checkChar(s, operants[i]);
                i++;
            }
            f = f == null ? new RationalNumberNode(float.Parse(s)) : f;
            return f;
        }

        static private Node checkChar(string s, char c)
        {
            Node f;
            if (s[0] == 'x')
            {
                f = new VariableNode();
                f.SetLeft(null);
                string p2 = s.Length <= 2 ? "" : s.Substring(2);//x and , are at least 2 symbols
                f.SetRight(getFunction(p2));
                return f;
            }

            if ((s[0] == 'n' || s[0] == 'r') && s.Length > 3)
            {
                double num = Convert.ToDouble(s.Substring(2, s.IndexOf(')') - 2));
                f = s[0] == 'n' ? new NaturalNumberNode(num) : (Node)new RationalNumberNode(float.Parse(num.ToString()));
                return f;
            }


            if ((s.Length > 1 && s[0] == c && s[1] == '('))
            {
                switch (c)
                {
                    case '+':
                        f = new PlusSignFunction(new NaturalNumberNode(0), new NaturalNumberNode(0));
                        break;
                    case '-':
                        f = new MinusSignFunction(new NaturalNumberNode(0), new NaturalNumberNode(0));
                        break;
                    case '*':
                        f = new MultiplicationSignFunction(new NaturalNumberNode(0), new NaturalNumberNode(0));
                        break;
                    case '/':
                        f = new DivisionSignFunction(new NaturalNumberNode(0), new NaturalNumberNode(0));
                        break;
                    case '^':
                        f = new PowerFunction(new NaturalNumberNode(0), new NaturalNumberNode(0));
                        break;
                    case 's':
                        f = new SinFunction(new NaturalNumberNode(0));
                        break;
                    case 'c':
                        f = new CosFunction(new NaturalNumberNode(0));
                        break;
                    case '!':
                        f = new FactorialFunction(new NaturalNumberNode(0));
                        break;
                    case 'l':
                        f = new LogarithmFunction(new NaturalNumberNode(0));
                        break;
                    case 'e':
                        f = new ExponentialFunction(new NaturalNumberNode(0));
                        break;
                    default:
                        f = null;//there was an error if it reaches this place
                        break;
                }
                s = s.Substring(2, s.Length - 3);
                int counter = 0;
                string p1 = "";
                string p2 = "";
                if (countChar("scle!'", c) > 0)
                {
                    p2 = s;
                }
                else
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i] == '(')
                        {
                            counter++;
                        }
                        else if (s[i] == ')')
                        {
                            counter--;
                        }

                        if (counter == 0 && s[i] == ',')
                        {
                            p1 = s.Substring(0, i);//cause s[i-1]=')',
                            p2 = s.Substring(i + 1);   //s[i]=',' and s[i+1]='('
                            break;
                        }
                    }
                }
                if (c == '!')
                {//cause I use the same Function classes for the normal notation
                    p1 = p2;
                    p2 = "";
                }

                if (c == '\'')
                {
                    f = ParserMc.getFunction(p2).ReturnDerivative();
                }
                else
                {
                    f.SetLeft(ParserMc.getFunction(p1));
                    f.SetRight(ParserMc.getFunction(p2));
                }
                return f;
            }
            return null;
        }

        static public int countChar(string s, char c)
        {
            int x = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    x++;
                }
            }
            return x;
        }
    }

}
