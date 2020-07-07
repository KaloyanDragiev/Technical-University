using System.Collections.Generic;

namespace LPP
{
    public class NANDOperator : BinaryOperator
    {
        public override Proposition Nandify()
        {
            return new NANDOperator()
            {
                Left = Left.Nandify(),
                Right = Right.Nandify()
            };
        }

        public override string ToGraph()
        {         
            return base.ToGraph() + "[label = \"%\"] "
                + Id + " -- " + Left.Id + " " + Id + " -- " + Right.Id
                + Left.ToGraph() + Right.ToGraph();
        }

        public override string ToInfixString()
        {
            return "(" + Left.ToInfixString() + "%" + Right.ToInfixString() + ")";
        }

        public override List<char> GenerateTruthTable()
        {
            List<char> values = new List<char>();
            List<char> leftValues = Left.GenerateTruthTable();
            List<char> rightValues = Right.GenerateTruthTable();
            for (int i = 0; i < leftValues.Count; i++)
            {
                if (leftValues[i] == '0' || rightValues[i] == '0')
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

        public override Proposition Copy()
        {
            return new NANDOperator()
            {
                Left = Left.Copy(),
                Right = Right.Copy()
            };
        }
    }
}
