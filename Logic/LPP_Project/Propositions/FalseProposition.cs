using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public class FalseProposition : Proposition
    {
        public FalseProposition()
        {
            this.Text = "0";
        }

        public override string ToString()
        {
            return "False";
        }
        
        public override bool CalculateTruthTable()
        {
            return false;
        }

        public override Proposition Nandify()
        {
            return this;
        }

        public override Proposition Copy()
        {
            return new FalseProposition();
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
