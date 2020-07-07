using System;
using System.Collections.Generic;

namespace LPP
{
    public class Predicate : Proposition, IComparable<Predicate>
    {
        public Predicate(char letter)
        {
            Letter = letter;
            Values = new List<char>();
            Variables = new List<char>();
        }

        public char Letter { get; set; }
        public List<char> Values { get; set; }
        public List<char> Variables { get; set; }

        private string getVariablesString()
        {
            if (Variables.Count > 0)
            {
                string vars = "(";
                foreach (char v in Variables)
                {
                    vars += v + ", ";
                }
                return vars.Substring(0, vars.Length - 2) + ")";
            }
            return "";
        }

        public override Proposition Nandify()
        {
            return new Predicate(Letter)
            {
                Values = Values,
                Variables = Variables
            };
        }

        public override string ToGraph()
        { 
            return " " + Id + "[label = \"" + Letter + getVariablesString() + "\"]";
        }

        public override string ToInfixString()
        {
            return Letter + getVariablesString();
        }

        public override List<char> GenerateTruthTable()
        {
            return Values;
        }

        public int CompareTo(Predicate other)
        {
            return Letter.CompareTo(other.Letter);
        }

        public override List<Predicate> GetPredicates()
        {
            return new List<Predicate> { this };
        }

        public override Proposition Copy()
        {
            return new Predicate(Letter)
            {
                Variables = new List<char>(Variables),
                Values = new List<char>(Values)
            };
        }
    }
}
