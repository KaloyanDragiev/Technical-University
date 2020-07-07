using System.Collections.Generic;

namespace LPP
{
    public abstract class Quantifier : Proposition
    {
        public Proposition Left { get; set; }
        public List<char> Variables { get; set; }

        public Quantifier()
        {
            Variables = new List<char>();
        }

        protected string getVariablesString()
        {
            if (Variables.Count > 0)
            {
                string vars = "";
                foreach (char v in Variables)
                {
                    vars += v + ", ";
                }
                return vars.Substring(0, vars.Length - 2) + "";
            }
            return "";
        }
        
        public override List<Predicate> GetPredicates()
        {
            return Left.GetPredicates();
        }   
    }
}
