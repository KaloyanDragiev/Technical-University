using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public class TrueProposition : Proposition
    {
        public TrueProposition()
        {
            this.Text = "1";
        }

        public override string ToString()
        {
            return "True";
        }

        public override bool CalculateTruthTable()
        {
            return true;
        }

        public override Proposition Nandify()
        {
            return this;
        }

        public override Proposition Copy()
        {
            return new TrueProposition();
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