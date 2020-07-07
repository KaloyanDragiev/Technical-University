using System.Collections.Generic;

namespace LPP
{
    public class NotOperator : Proposition
    {
        public Proposition Left { get; set; }

        public NotOperator() { }

        public NotOperator(Proposition left)
        {
            Left = left;
        }

        public override Proposition Nandify()
        {
            return new NANDOperator()
            {
                Left = Left.Nandify(),
                Right = Left.Nandify()
            };
        }

        public override string ToGraph()
        {
            if (Left is Predicate left)
            {
                Left = left.Copy();
            }
            return " " + Id + "[label = \"¬\"] " + Id + " -- " + Left.Id + " " + Left.ToGraph();
        }

        public override string ToInfixString()
        {
            return "¬" + Left.ToInfixString();
        }

        public override List<char> GenerateTruthTable()
        {
            List<char> values = new List<char>();
            List<char> leftValues = Left.GenerateTruthTable();
            foreach (char c in leftValues)
            {
                if (c == '0')
                {
                    values.Add('1');
                }
                else
                {
                    values.Add('0');
                }
            }
            return values;
        }

        public override List<Predicate> GetPredicates()
        {
            return Left.GetPredicates();
        }

        public override Proposition Copy()
        {
            return new NotOperator(Left.Copy());
        }
    }
}
