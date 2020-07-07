using System;
using System.Collections.Generic;

namespace LPP
{
    public class InputReader
    {
        private Proposition root;
        private string input;
        private int nrPredicates;
        private List<Predicate> predicates;
        private bool isProposition = true;
        private List<char> boundVariables = new List<char>();
        private List<char> unboundVariables = new List<char>();

        public InputReader(string input)
        {
            this.input = input;
            nrPredicates = 0;
            predicates = new List<Predicate>();
        }

        public bool IsProposition { get => isProposition; set => isProposition = value; }

        private Proposition GeneratePredicate(Proposition func, int i)
        {          
            input = input.Substring(i);
            if (func is Predicate pred)
            {
                if (input.Length > 1 && input[i + 1] == '(')
                {
                    while (input[i] != ')')
                    {
                        i++;
                        if (Char.IsLower(input[i]))
                        {
                            pred.Variables.Add(input[i]);
                            if (!boundVariables.Contains(input[i]) && !unboundVariables.Contains(input[i]))
                            {
                                unboundVariables.Add(input[i]);
                            }
                            i++;
                        }
                    }
                }
                input = input.Substring(i);
                foreach (Predicate p in predicates)
                {
                    if (p.Letter == pred.Letter)
                    {    
                        if (isProposition)
                        {
                            return p;
                        }
                        if (p.Variables.Count == pred.Variables.Count)
                        {
                            return func;
                        }
                        return null;
                    }
                }
                predicates.Add(pred);
            }          
            if (root == null)
            {
                root = func;
            }
            return func;
        }

        private Proposition GenerateBinary(BinaryOperator func, int i)
        {
            if (input[i] == '(')
            {
                input = input.Substring(i);
                i = 0;
                func.Left = ParseInput();
                while (input[i] != ',')
                {
                    i++;
                }
                input = input.Substring(i);
                func.Right = ParseInput();
                if (i <= input.Length)
                {
                    input = input.Substring(i);
                }
                if (root == null)
                {
                    root = func;
                }
                return func;
            }
            return null;
        }

        private Proposition GenerateNot(NotOperator func, int i)
        {           
            if (input[i] == '(')
            {
                input = input.Substring(i);
                i = 0;
                func.Left = ParseInput();
                while (input[i] != ')')
                {
                    i++;
                }
                input = input.Substring(i);
                if (root == null)
                {
                    root = func;
                }
                return func;
            }
            return null;
        }

        private Quantifier GenerateQuantifier(Quantifier func, int i)
        {
            while (input[i] != '.')
            {
                if (Char.IsLower(input[i]))
                {
                    func.Variables.Add(input[i]);
                }
                i++;
            }
            i++;
            boundVariables.AddRange(func.Variables);
            if (input[i] == '(')
            {
                input = input.Substring(i);
                i = 0;
                func.Left = ParseInput();
                while (input[i] != ')')
                {
                    i++;
                }
                input = input.Substring(i);
                boundVariables = new List<char>();
                if (root == null)
                {
                    root = func;
                }
                return func;
            }
            return null;
        }

        private Proposition ParseInput()
        {
            for (int i = 0; i < input.Length;)
            {              
                if (Char.IsUpper(input[i]))
                {
                    return GeneratePredicate(new Predicate(input[i]), i);
                }
                else {
                    switch (input[i])
                    {
                        case '0':
                            return GeneratePredicate(new False((int)Math.Pow(2, nrPredicates)), i);
                        case '1':
                            return GeneratePredicate(new True((int)Math.Pow(2, nrPredicates)), i);
                        case '~':
                            return GenerateNot(new NotOperator(), i + 1);
                        case '>':
                            return GenerateBinary(new ImplicationOperator(), i + 1);
                        case '=':
                            return GenerateBinary(new BiImplicationOperator(), i + 1);
                        case '&':
                            return GenerateBinary(new AndOperator(), i + 1);
                        case '|':
                            return GenerateBinary(new OrOperator(), i + 1);
                        case '%':
                            return GenerateBinary(new NANDOperator(), i + 1);
                        case '@':
                            return GenerateQuantifier(new UniversalQuantifier(), i + 1);
                        case '!':
                            return GenerateQuantifier(new ExistentialQuantifier(), i + 1);
                        default:                          
                            input = input.Substring(i + 1);
                            break;
                    }
                }
            }
            return root;
        }

        public Proposition CheckInput()
        {
            List<char> letters = new List<char>();
            int opened = 0;
            int closed = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    opened++;
                }
                if (input[i] == ')')
                {
                    closed++;
                }
                if (Char.IsUpper(input[i]))
                {
                    if (!letters.Contains(input[i]))
                    {
                        letters.Add(input[i]);
                    }                  
                }
                if ((input[i] == '@' || input[i] == '!') && IsProposition)
                {
                    IsProposition = false;
                }
            }
            if (opened == closed)
            {
                nrPredicates = letters.Count;
                return ParseInput();
            }
            throw new Exception("Mismatched parentheses");
        }

        public List<Predicate> GetPredicates()
        {
            return predicates;
        }

        public List<char> GetUnboundVariables()
        {
            return unboundVariables;
        }
    }
}
