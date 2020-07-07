using System.Collections.Generic;

namespace LPP
{
    public class True : Proposition
    {
        public True(int size)
        {
            Values = new List<char>();
            for (int i = 0; i < size; i++)
            {
                Values.Add('1');
            }
        }

        public List<char> Values { get; set; }

        public override Proposition Nandify()
        {
            return new True(Values.Count)
            {
                Values = Values
            };
        }

        public override string ToGraph()
        {
            return " " + Id + "[label = \"True\"]";
        }

        public override string ToInfixString()
        {
            return "True";
        }

        public override List<char> GenerateTruthTable()
        {
            return Values;
        }

        public override List<Predicate> GetPredicates()
        {
            return null;
        }

        public override Proposition Copy()
        {
            return new True(Values.Count);
        }
    }
}
