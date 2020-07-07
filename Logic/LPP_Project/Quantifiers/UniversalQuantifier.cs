namespace LPP_Project.Quantifiers
{
    using Propositions;
    using System.Collections.Generic;

    public class UniversalQuantifier : Quantifier
    {
        public UniversalQuantifier()
        {
            this.Text = "U";
        }
        public override string ToString()
        {
            return "@" + this.GetVariablesString() + "[" + this.LeftNode.ToString() + "]";
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
            return new UniversalQuantifier
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
