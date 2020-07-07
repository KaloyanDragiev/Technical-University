namespace LPP_Project.Propositions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    public class VariableProposition : Proposition, IComparable<VariableProposition>
    {
        private static readonly List<char> ListOfLetters = "GHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();//01
        public static List<VariableProposition> GeneratedList = new List<VariableProposition>();

        private string variable;
        public bool Value { get; set; }
        public int ValueDP { get; set; }
        public List<char> Variables { get; set; }

        public static int Counter = 0;

        public VariableProposition(string variable)
        {
            this.Text = variable;
            this.variable = variable;
            this.Variables = new List<char>();
            this.ValueDP = -2;
        }

        public void AddVariable()
        {
            //check if out of range counter   
            string letter = ListOfLetters[Counter].ToString();
            Counter++;
            this.Text = letter;
            this.variable = letter;
            GeneratedList.Add(this);
        }

        public static void ClearListOfVariables()
        {
            GeneratedList = new List<VariableProposition>();
        }

        public override string ToString()
        {
            return this.variable;
        }

        public override bool CalculateTruthTable()
        {
            return this.Value;
        }
            
        public override Proposition Nandify()
        {
            return this;
        }

        public int CompareTo(VariableProposition other)
        {
            return string.Compare(this.Text, other.Text, StringComparison.Ordinal);
        }

        public override Proposition Copy()
        {
            return new VariableProposition(this.variable);
        }

        public override MultiAnd Cnf()
        {
            MultiOr multiOr = new MultiOr();
            multiOr.Propositions.Add(this);

            MultiAnd multiAnd = new MultiAnd();
            multiAnd.AddProposition(multiOr);

            return multiAnd;
        }

        public override Proposition Simplify()
        {
            return this;
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            return this;
        }
    }
}
