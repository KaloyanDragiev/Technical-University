namespace LPP_Project.Quantifiers
{
    using Propositions;
    using System.Collections.Generic;

    public abstract class Quantifier : Proposition
    {
        public List<char> Variables { get; set; }

        protected Quantifier()
        {
            this.Variables = new List<char>();
        }

        protected string GetVariablesString()
        {
            if (this.Variables.Count > 0)
            {
                string vars = "";
                foreach (char v in this.Variables)
                {
                    vars += v + ", ";
                }
                return vars.Substring(0, vars.Length - 2) + "";
            }
            return "";
        }
    }
}
