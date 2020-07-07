using System.Collections.Generic;

namespace LPP
{
    public class False : Proposition
    {
        public False(int size)
        {
            Values = new List<char>();
            for (int i = 0; i < size; i++)
            {
                Values.Add('0');
            }
        }

        public List<char> Values { get; set; }

        public override Proposition Nandify()
        {
            return new False(Values.Count)
            {
                Values = Values
            };
        }

        public override string ToGraph()
        {
            return " " + Id + "[label = \"False\"]";
        }

        public override string ToInfixString()
        {
            return "False";
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
            return new False(Values.Count);
        }
    }
}
