namespace LPP_Project
{
    using Propositions;
    using System.Collections.Generic;

    public class TableuElement
    {
        public bool Status { get; set; }
        public TableuElement ParentNode { get; set; }
        public Proposition ElemetValue { get; set; }

        public TableuElement(Proposition multiAnd)
        {
            this.ElemetValue = multiAnd;
        }

        public List<Proposition> ApplyAlfaRules()
        {
            List<Proposition> resultElements = new List<Proposition>();

            if (this.ElemetValue is AndProposition)
            {
                AndProposition and = (AndProposition)this.ElemetValue;
                resultElements.Add(and);
            }

            return resultElements;
        }


        public List<Proposition> ApplyBetaRules()
        {
            List<Proposition> resultElements = new List<Proposition>();

            if (this.ElemetValue is AndProposition)
            {
                AndProposition and = (AndProposition)this.ElemetValue;
                resultElements.Add(and);
            }

            return resultElements;
        }
    }
}
