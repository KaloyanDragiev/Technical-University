using System.Collections.Generic;

namespace LPP
{
    public class BiImplicationOperator : BinaryOperator
    {
        public override Proposition Nandify()
        {
            ImplicationOperator impl1 = new ImplicationOperator()
            {
                Left = Left,
                Right = Right
            };
            ImplicationOperator impl2 = new ImplicationOperator()
            {
                Left = Right,
                Right = Left
            };
            AndOperator and = new AndOperator()
            {
                Left = impl1,
                Right = impl2
            };
            return and.Nandify();
        }

        public override string ToGraph()
        {
            return base.ToGraph() + "[label = \"<=>\"] "
                + Id + " -- " + Left.Id + " " + Id + " -- " + Right.Id
                + Left.ToGraph() + Right.ToGraph();
        }

        public override string ToInfixString()
        {
            return "(" + Left.ToInfixString() + "<=>" + Right.ToInfixString() + ")";
        }

        public override List<char> GenerateTruthTable()
        {
            List<char> values = new List<char>();
            List<char> leftValues = Left.GenerateTruthTable();
            List<char> rightValues = Right.GenerateTruthTable();
            for (int i = 0; i < leftValues.Count; i++)
            {
                if ((leftValues[i] == '0' || rightValues[i] == '1') && (leftValues[i] == '1' || rightValues[i] == '0'))
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
            return new BiImplicationOperator()
            {
                Left = Left.Copy(),
                Right = Right.Copy()
            };
        }
    }
}
