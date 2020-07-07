using System.Runtime.CompilerServices;

namespace LPP_Project
{
    using System;
    using System.Linq;
    using Quantifiers;
    using Propositions;
    using System.Collections.Generic;

    public static class ParseExpression
    {
        private static List<VariableProposition> listOfVariables = new List<VariableProposition>();
        //private static readonly List<char> ListOfLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();//01
        private static readonly List<char> ListOfLetters = "ABCDEF".ToCharArray().ToList();//01
        private static readonly List<char> ListOfPropositions = "&|~>=%".ToCharArray().ToList();
        private static readonly Random Random = new Random();
        private static List<char> boundVariables = new List<char>();
        private static readonly List<char> UnboundVariables = new List<char>();
        private static readonly List<VariableProposition> Predicates = new List<VariableProposition>();
        public static bool IsProposition = true;

        public static List<VariableProposition> GetVariables()
        {
            listOfVariables = listOfVariables.OrderBy(x => x.Text).ToList();
            return listOfVariables;
        }

        public static void ClearListOfVariables()
        {
            listOfVariables = new List<VariableProposition>();
        }

        public static Proposition Parse(ref string input)
        {
            if (input.Length == 0)
                return null;

            char operand = input[0];
            input = input.Substring(1);

            Proposition firstNode;
            if (operand == '(' || operand == ')')
            {
                operand = input[0];
                if (operand != '!' || operand != '@')
                {
                    input = input.Substring(1);
                }
            }

            switch (operand)
            {
                case '>':
                    ReadValDelete(ref input, '(');
                    firstNode = Parse(ref input);

                    ReadValDelete(ref input, ',');

                    Proposition secondNode = Parse(ref input);
                    ReadValDelete(ref input, ')');

                    return new ImplicationProposition(firstNode, secondNode);

                case '=':
                    ReadValDelete(ref input, '(');
                    firstNode = Parse(ref input);

                    ReadValDelete(ref input, ',');

                    Proposition secondNode2 = Parse(ref input);
                    ReadValDelete(ref input, ')');

                    return new BiImplicationProposition(firstNode, secondNode2);

                case '&':
                    ReadValDelete(ref input, '(');
                    firstNode = Parse(ref input);

                    ReadValDelete(ref input, ',');

                    Proposition secondNode3 = Parse(ref input);
                    ReadValDelete(ref input, ')');

                    return new AndProposition(firstNode, secondNode3);

                case '|':
                    ReadValDelete(ref input, '(');
                    firstNode = Parse(ref input);

                    ReadValDelete(ref input, ',');

                    Proposition secondNode4 = Parse(ref input);
                    ReadValDelete(ref input, ')');

                    return new OrProposition(firstNode, secondNode4);

                case '%':
                    ReadValDelete(ref input, '(');
                    firstNode = Parse(ref input);

                    ReadValDelete(ref input, ',');

                    Proposition secondNode5 = Parse(ref input);
                    ReadValDelete(ref input, ')');

                    //¬(A⋀B)
                    return new NegationProposition(new AndProposition(firstNode, secondNode5));

                case '~':
                    ReadValDelete(ref input, '(');

                    firstNode = Parse(ref input);

                    if (IsProposition)
                    {
                        ReadValDelete(ref input, ')');
                    }

                    return new NegationProposition(firstNode);

                case '@':
                    //!y.(P(x,y))
                    IsProposition = false;
                    UniversalQuantifier unvQuan = new UniversalQuantifier();
                    int i = 0;
                    while (input[i] != '.')
                    {
                        if (char.IsLower(input[i]))
                        {
                            unvQuan.Variables.Add(input[i]);
                        }
                        i++;
                    }
                    i++;
                    boundVariables.AddRange(unvQuan.Variables);

                    if (input[i] == '(')
                    {
                        input = input.Substring(i);
                        i = 0;
                        unvQuan.LeftNode = Parse(ref input);
                        if (input[i] != ',')
                        {

                            while (input[i] != ')')
                            {
                                i++;
                            }

                            if (i == 0)
                            {
                                while (input[i] != ',')
                                {
                                    if (input.Length - 1 == i)
                                    {
                                        break;
                                    }
                                    // >(!x.(@y.(P(x,y))),@q.(!p.(P(p,q))))
                                    i++;
                                }
                            }
                        }
                        input = input.Substring(i);
                        boundVariables = new List<char>();
                        return unvQuan;
                    }
                    return null;

                case '!':
                    //!y.(P(x,y))
                    IsProposition = false;
                    ExistentialQuantifier exsQuan = new ExistentialQuantifier();
                    int j = 0;
                    while (input[j] != '.')
                    {
                        if (Char.IsLower(input[j]))
                        {
                            exsQuan.Variables.Add(input[j]);
                        }
                        j++;
                    }
                    j++;
                    boundVariables.AddRange(exsQuan.Variables);

                    if (input[j] == '(')
                    {
                        input = input.Substring(j);
                        j = 0;
                        exsQuan.LeftNode = Parse(ref input);
                        if (input[j] != ',')
                        {
                            while (input[j] != ')')
                            {
                                j++;
                            }

                            if (j == 0)
                            {
                                while (input[j] != ',')
                                {
                                    if (input.Length - 1 == j)
                                    {
                                        break;
                                    }
                                    // >(!x.(@y.(P(x,y))),@q.(!p.(P(p,q))))
                                    j++;
                                }
                            }
                        }
                        input = input.Substring(j);
                        boundVariables = new List<char>();
                        return exsQuan;
                    }
                    IsProposition = false;
                    return null;

                case '0':
                    return new FalseProposition();
                case '1':
                    return new TrueProposition();

                default:
                    if (IsProposition)
                    {
                        return AddVariableFunction(operand);
                    }
                    return GeneratePredicate(ref input, new VariableProposition(operand.ToString()), 0);
            }
        }

        public static VariableProposition AddVariableFunction(char text)
        {
            //if (!char.IsLetter(text) || char.IsLower(text))
            //{
            //    throw new Exception("The given expression contains invalid symbols or missing operand!");
            //}

            foreach (VariableProposition variableFunction in listOfVariables)
            {
                if (variableFunction.Text == text.ToString())
                {
                    return variableFunction;
                    //return new VariableMultiAnd(text.ToString());//If the same return new var
                }
            }

            VariableProposition newVariableFunction = new VariableProposition(text.ToString());
            listOfVariables.Add(newVariableFunction);

            return newVariableFunction;
        }
        
        private static Proposition GeneratePredicate(ref string input, VariableProposition func, int i)
        {
            input = input.Substring(i);
            if (func is VariableProposition pred)
            {
                if (input.Length > 1 && input[i] == '(')
                {
                    while (input[i] != ')')
                    {
                        i++;
                        if (char.IsLower(input[i]))
                        {
                            pred.Variables.Add(input[i]);
                            if (!boundVariables.Contains(input[i]) && !UnboundVariables.Contains(input[i]))
                            {
                                UnboundVariables.Add(input[i]);
                            }
                            i++;
                        }
                    }
                }
                input = input.Substring(i);
                foreach (VariableProposition p in Predicates)
                {
                    if (p.Value == pred.Value)
                    {
                        if (IsProposition)
                        {
                            return AddVariableFunction(p.Value.ToString()[0]);
                        }
                        if (p.Variables.Count == pred.Variables.Count)
                        {
                            return AddVariableFunction(func.Value.ToString()[0]);
                        }
                        return null;
                    }
                }
                Predicates.Add(pred);
            }
            return AddVariableFunction((func != null && func.Value).ToString()[0]);
        }

        private static void ReadValDelete(ref string input, char c)
        {
            if (input == "")
            {
                throw new Exception("Missing Right Bracket!");
            }

            if (input[0] != c)
            {
                switch (c)
                {
                    case '(':
                        throw new Exception("Missing Left Bracket!");
                    case ',':
                        throw new Exception("Separator Expected!");
                    case ')':
                        throw new Exception("Missing Right Bracket!");
                }
            }

            input = input.Substring(1);
        }

       public static long TwoPowX(int power)
       {
           return 1L << power;
       }

        public static string CreateRandomProposition()
        {
            int dives = Random.Next(4, 10);
            int maxPropositions = (dives - 1) ; // / 2
            return CreateRandomPropositionString(dives, maxPropositions);
        }

        private static string CreateRandomPropositionString(int dives, int maxPropositions)
        {
            if (dives == 0)
            {
                char randomVariable = ListOfLetters[Random.Next(0, 5 + 1)];
                //if (Random.Next(1, 11) <= 2)
                //{
                //    randomVariable = Random.Next(0, 2) == 0 ? '0' : '1';
                //}

                return randomVariable.ToString();
            }

            int index = Random.Next(0, ListOfPropositions.Count);
            string propositionType = ListOfPropositions[index].ToString();
            dives--;

            if (propositionType == "~")
            {
                propositionType += "(";
                propositionType += CreateRandomPropositionString(dives, maxPropositions);
                propositionType += ")";
            }
            else
            {
                int dives2 = Random.Next(0, dives + 1);
                int dives3 = dives - dives2;
                propositionType += "(";
                propositionType += CreateRandomPropositionString(dives2, maxPropositions);
                propositionType += ",";
                propositionType += CreateRandomPropositionString(dives3, maxPropositions);
                propositionType += ")";
            }

            return propositionType;
        }

        public static List<VariableProposition> GetAllVariables()
        {
            var newList = new List<VariableProposition>();

            foreach (VariableProposition variableProposition in listOfVariables)
            {
                newList.Add(variableProposition);
            }
            foreach (VariableProposition variableProposition in VariableProposition.GeneratedList)
            {
                newList.Add(variableProposition);
            }

            return newList;
        }
    }
}
