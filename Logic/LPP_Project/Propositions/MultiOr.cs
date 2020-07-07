namespace LPP_Project.Propositions
{
    using System.Collections.Generic;

    public class MultiOr : Proposition  
    {
        public List<Proposition> Propositions { get; set; }
        public bool Useless { get; set; }

        public MultiOr()
        {
            this.Propositions = new List<Proposition>();
            this.Text = "V";
        }
        
        public override bool CalculateTruthTable()
        {
            List<bool> array = new List<bool>();
            foreach (Proposition proposition in this.Propositions)
            {
                array.Add(proposition.CalculateTruthTable());
            }
            //0 1 1 0 0 1
            bool result = array[0];
            for (int i = 1; i < array.Count; i++)
            {
                result = result || array[i];
            }

            return result;
        }

        public override Proposition Nandify()
        {
            return this;
        }

        public override Proposition Copy()
        {
            MultiOr multiOr = new MultiOr();
            foreach (Proposition proposition in this.Propositions)
            {
                multiOr.Propositions.Add(proposition.Copy());
            }

            return multiOr;
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

        public override string ToString()
        {
            var result = "(";
            foreach (var p in this.Propositions)
            {
                result += p.ToString() + " v ";
            }

            if (result.Length != 1)
            {
                result = result.Remove(result.Length - 3);
            }

            result += ")";

            if (result.Length == 3 || (result.Length == 6 && result.Contains("¬"))|| 
                (result.Length == 7 && result.Contains("False")) ||
                (result.Length == 6 && result.Contains("True")))//(A) (False) (True) (~(A))
            {
                return result.Substring(1, result.Length - 2);
            }

            return result;
        }

        public void RemoveUseless()
        {
            for (int i = 0; i < this.Propositions.Count; i++)
            {
                for (int j = 1; j < this.Propositions.Count; j++)
                {
                    var a = "¬(" + this.Propositions[i].ToString() + ")";
                    var b = this.Propositions[j].ToString();
                    var c = "¬(" + this.Propositions[j].ToString() + ")";
                    var d = this.Propositions[i].ToString();

                    if (a == b || c == d)
                    {
                        this.Useless = true;
                    }
                }
            }
        }

    }
}
