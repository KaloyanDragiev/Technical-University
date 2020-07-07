using System.Collections.Generic;

namespace LPP
{
    public class OrOperator : BinaryOperator
    {
        public override Proposition Nandify()
        {
            NANDOperator nand1 = new NANDOperator()
            {
                Left = Left.Nandify(),
                Right = Left.Nandify()
            };
            NANDOperator nand2 = new NANDOperator()
            {
                Left = Right.Nandify(),
                Right = Right.Nandify()
            };
            return new NANDOperator()
            {
                Left = nand1,
                Right = nand2
            };
        }

        public override string ToGraph()
        {
            return base.ToGraph() + "[label = \"\\\\/\"] "
                + Id + " -- " + Left.Id + " " + Id + " -- " + Right.Id
                + Left.ToGraph() + Right.ToGraph();
        }

        public override string ToInfixString()
        {
            return "(" + Left.ToInfixString() + "\\/" + Right.ToInfixString() + ")";
        }

        public override List<char> GenerateTruthTable()
        {
            List<char> values = new List<char>();
            List<char> leftValues = Left.GenerateTruthTable();
            List<char> rightValues = Right.GenerateTruthTable();
            for (int i = 0; i < leftValues.Count; i++)
            {
                if (leftValues[i] == '1' || rightValues[i] == '1')
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
            return new OrOperator()
            {
                Left = Left.Copy(),
                Right = Right.Copy()
            };
        }
    }
}
