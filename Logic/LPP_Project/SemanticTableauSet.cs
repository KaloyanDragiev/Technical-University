namespace LPP_Project
{
    using System;
    using Propositions;
    using System.Collections.Generic;
    
    public class SemanticTableauSet
    {
        public Proposition Root { get; set; }
        public TableuElement Node { get; set; }
        public bool IsTautology { get; set; }
        public List<SemanticTableauSet> ListOfSets { get; set; }
        public List<TableuElement> ListOfElements { get; set; }

        public SemanticTableauSet(Proposition root)
        {
            this.Root =  new NegationProposition(root);
        }

        public SemanticTableauSet(List<TableuElement> elements)
        {
            this.ListOfElements = elements;
        }

        public bool CheckTautology()
        {
            if (this.ListOfSets.Count > 0)
            {
                bool tautology = true;
                foreach (SemanticTableauSet set in this.ListOfSets)
                {
                    if (set.CheckTautology() == false)
                    {
                        tautology = false;
                    }
                }

                this.IsTautology = tautology;
                return tautology;
            }
            this.IsTautology = this.CheckTautologyWithOwnElement();

            return this.IsTautology;
        }

        public bool CheckTautologyWithOwnElement()
        {
            //MultiAnd temp;
            for (int i = 0; i < this.ListOfElements.Count; i++)
            {
                   
            }

            return false;
        }

        public bool TryApplyAlfaRules()
        {
            List<Proposition> result;
            foreach (TableuElement tableuElement in this.ListOfElements)
            {
                result = tableuElement.ApplyAlfaRules();
                if (result.Count > 0)
                {
                    this.AddNewSet(result, tableuElement);
                    return true;
                }
            }
            return false;
        }

        public bool TryApplyBetaRules()
        {
            List<Proposition> result;
            foreach (TableuElement tableuElement in this.ListOfElements)
            {
                result = tableuElement.ApplyBetaRules();
                if (result.Count > 0)
                {
                    if (result.Count != 2)
                    {
                        throw new Exception("Not Possible!");
                    }

                    this.AddNewSet(new List<Proposition> { result[0] }, tableuElement);
                    this.AddNewSet(new List<Proposition> { result[1] }, tableuElement);
                    return true;
                }
            }
            return false;
        }

        public void AddNewSet(List<Proposition> propositions, TableuElement elemet)
        {
            List<TableuElement> temp =new List<TableuElement>();
            foreach (TableuElement tableuElement in this.ListOfElements)
            {
                if (tableuElement != elemet)
                {
                    temp.Add(tableuElement);
                }
            }

            foreach (Proposition proposition in propositions)
            {
                bool sameFound = false;
                foreach (TableuElement element in temp)
                {
                    if (element.ElemetValue == proposition)
                    {
                        sameFound = true;
                        break;
                    }
                }

                if (!sameFound)
                {
                    TableuElement newElement = new TableuElement(proposition);
                    temp.Add(newElement);
                }
            }

            SemanticTableauSet newSet = new SemanticTableauSet(temp);
            this.ListOfSets.Add(newSet);
        }
    }
}
