using System.Collections.Generic;

namespace LPP
{
    public class ExistentialQuantifier : Quantifier
    {     
        public override string ToGraph()
        {
            return " " + Id + "[label = \"!" + getVariablesString() + "\"] " + Id + " -- " + Left.Id + " " + Left.ToGraph();
        }

        public override string ToInfixString()
        {
            return "!" + getVariablesString() + "[" + Left.ToInfixString() + "]";
        }

        public override List<char> GenerateTruthTable()
        {
            return null;
        }

        public override Proposition Nandify()
        {
            return null;
        }

        public override Proposition Copy()
        {
            return new ExistentialQuantifier()
            {
                Left = Left.Copy(),
                Variables = new List<char>(Variables)
            };
        }       
    }
}
