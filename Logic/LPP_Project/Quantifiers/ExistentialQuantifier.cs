namespace LPP_Project.Quantifiers
{
    using Propositions;
    using System.Collections.Generic;

    public class ExistentialQuantifier : Quantifier
    {
        public ExistentialQuantifier()
        {
            this.Text = "E";
        }

        public override string ToString()
        {
            return "!" + this.GetVariablesString() + "[" + this.LeftNode.ToString() + "]";
        }

        public override bool CalculateTruthTable()
        {
            return false;
        }

        public override Proposition Nandify()
        {
            return null;
        }

        public override Proposition Copy()
        {
            return new ExistentialQuantifier
            {
                LeftNode = this.LeftNode.Copy(),
                Variables = new List<char>(this.Variables)
            };
        }

        public override MultiAnd Cnf()
        {
            return null;
        }

        public override Proposition Simplify()
        {
            return this;
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            throw new System.NotImplementedException();
        }
    }
}
