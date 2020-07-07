namespace LPP_Project.Propositions
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Forms.VisualStyles;

    public class MultiAnd : Proposition
    {
        public List<MultiOr> Propositions { get; set; }

        public MultiAnd()
        {
            this.Propositions = new List<MultiOr>();
            this.Text = "^";
        }

        public void AddProposition(MultiOr mO) => this.Propositions.Add(mO);

        public MultiAnd JoinAnd(MultiAnd prop)
        {
            MultiAnd newMultiAnd = new MultiAnd();

            foreach (MultiOr mOProposition in prop.Propositions)
            {
                newMultiAnd.AddProposition(mOProposition);
            }

            foreach (MultiOr proposition in this.Propositions)
            {
                newMultiAnd.AddProposition(proposition);
            }

            if (newMultiAnd.Propositions.Count != 0 && newMultiAnd.Propositions[0].Propositions.Count != 0)
            {
                newMultiAnd = this.RemoveCases(newMultiAnd);// A v A = A | A v ~A = False | A v True = A | A v False = False
            }

            return newMultiAnd;
        }

        public MultiAnd RemoveCases(MultiAnd prop)
        {
            //TODO: Remove Duplicates: A ^ (A v C) = (A V C)
            //TODO: Remove Duplicates: ~A ^ (A v C) = C 

            //(A v B) ^ (A v C) = A ^ B ^ C 
            //(! a || ! d || ! g) && (a || d || ! e) && (a || ! e || g) && (! d || e || ! g)
            //(¬A v ¬D v ¬G) ^ (A v D v ¬E) ^ (A v ¬E v G) ^ (¬D v E v ¬G)
            MultiAnd toRemove = new MultiAnd();
            bool isFalse = false;

            for (int i = 0; i < prop.Propositions.Count; i++)
            {
                if (prop.Propositions[i].ToString() == "True")// A ^ True = A
                {
                    toRemove.Propositions.Add(prop.Propositions[i]);
                }
                if (prop.Propositions[i].ToString() == "False")// A ^ False = False
                {
                    isFalse = true;
                }
            }

            prop.Propositions.RemoveAll(toRemove.Propositions.Contains);

            if (isFalse)
            {
                MultiAnd and = new MultiAnd();
                MultiOr or = new MultiOr();
                or.Propositions.Add(new FalseProposition());
                and.AddProposition(or);
                return and;
            }

            toRemove = new MultiAnd();

            for (int i = 0; i < prop.Propositions.Count - 1; i++)
            {
                for (int j = i + 1; j < prop.Propositions.Count; j++)
                {
                    var a = prop.Propositions[i].ToString();
                    var b = prop.Propositions[j].ToString();
                    if (a == b)// A ^ A = A
                    {
                        if (a.ToString().Contains("True"))//False ^ False
                        {
                            MultiAnd and = new MultiAnd();
                            MultiOr or = new MultiOr();
                            or.Propositions.Add(new TrueProposition());
                            and.AddProposition(or);
                            return and;
                        }
                        else if (a.ToString().Contains("False"))//True  ^ True
                        {
                            MultiAnd and = new MultiAnd();
                            MultiOr or = new MultiOr();
                            or.Propositions.Add(new FalseProposition());
                            and.AddProposition(or);
                            return and;
                        }
                        else
                        {// A ^ A = A
                            toRemove.Propositions.Add(prop.Propositions[j]);
                        }
                    }
                    if (a == "¬(" + b + ")" || "¬(" + a + ")" == b)// A ^ ~A = False
                    {//commented because on Davis putman last case [F f] is correct type
                        //if (!DavidPuttnam.In)
                        //{
                        //    MultiAnd and = new MultiAnd();
                        //    MultiOr or = new MultiOr();
                        //    or.Propositions.Add(new FalseProposition());
                        //    and.AddProposition(or);
                        //    return and;
                        //}
                    }
                }
            }

            prop.Propositions.RemoveAll(toRemove.Propositions.Contains);

            if (prop.Propositions.Count < 1)
            {
                MultiAnd and = new MultiAnd();
                MultiOr or = new MultiOr();
                or.Propositions.Add(new TrueProposition());
                and.AddProposition(or);
                return and;
            }
            
            return prop;
        }

        public MultiOr JoinOr(MultiAnd prop)
        {
            MultiOr newMultiOr = new MultiOr();

            foreach (Proposition mOProposition in ((MultiOr)prop.Propositions[0]).Propositions)
            {
                newMultiOr.Propositions.Add(mOProposition);
            }

            foreach (MultiOr p in this.Propositions)
            {
                foreach (Proposition p2 in p.Propositions)
                {
                    newMultiOr.Propositions.Add(p2);
                }
            }

            newMultiOr = this.RemoveDuplicate(newMultiOr);
            return newMultiOr;
        }

        public MultiAnd Distribution(MultiAnd prop)
        {
            //check for False v False
            if (prop.ToString() == "False" && this.ToString() == "False")
            {
                MultiAnd f = new MultiAnd();
                MultiOr fr = new MultiOr();
                fr.Propositions.Add(new FalseProposition());
                f.Propositions.Add(fr);
                return f;
            }

            MultiAnd newMultiAnd = new MultiAnd();

            foreach (MultiOr p in this.Propositions)//(A ^ B) V (C ^ D)
            {
                MultiOr newMultiOr = new MultiOr();
                foreach (Proposition p2 in p.Propositions)
                {
                    if (!p2.ToString().Contains("False"))
                        newMultiOr.Propositions.Add(p2);// A
                }

                foreach (MultiOr prop1 in prop.Propositions)
                {
                    foreach (Proposition prop2 in prop1.Propositions)//C ^ D
                    {
                        if (!prop2.ToString().Contains("False"))
                            newMultiOr.Propositions.Add(prop2);//C
                    }
                    //check if duplicate
                    newMultiOr = this.RemoveDuplicate(newMultiOr);//A V A = A

                    if (this.RemoveTrue(newMultiOr))
                    {// A V True = True
                        newMultiAnd.Propositions.Add(new MultiOr() { Propositions = new List<Proposition>() { new TrueProposition() } });
                    }
                    else if (this.RemoveNegationDuplicate(newMultiOr))
                    {
                        if (newMultiOr.Propositions.Count > 1)//A V A = A
                        {// A V False = A 
                            if (newMultiOr.Propositions[0].ToString() == "False")
                                newMultiOr.Propositions.Remove(newMultiOr.Propositions[0]);
                            else if (newMultiOr.Propositions[1].ToString() == "False")
                                newMultiOr.Propositions.Remove(newMultiOr.Propositions[1]);
                        }

                        newMultiAnd.Propositions.Add(newMultiOr);//A V B 
                    }
                    else
                    {//A V ~A = True A v A =True
                        //newMultiAnd.Propositions.Add(new MultiOr() { Propositions = new List<Proposition>() { new TrueProposition() } });
                    }
                    
                    newMultiOr = new MultiOr();// set newMultiOr to A again
                    for (int i = 0; i < p.Propositions.Count; i++)
                    {
                        newMultiOr.Propositions.Add(p.Propositions[i]);
                    }
                }
            }

            if (this.Propositions.Count == 0)
            {
                return prop;
            }

            if (newMultiAnd.Propositions.Count == 0 || newMultiAnd.Propositions[0].Propositions.Count == 0)
            {
                newMultiAnd.Propositions.Add(new MultiOr() { Propositions = new List<Proposition>() { new TrueProposition() } });
            }

            return newMultiAnd;
        }

        public MultiOr RemoveDuplicate(MultiOr mo)
        {
            MultiOr temp = new MultiOr();

            foreach (Proposition p in mo.Propositions)
            {
                bool duplicate = false;
                foreach (Proposition tempProposition in temp.Propositions)
                {
                    if (tempProposition.ToString() == p.ToString())
                    {//&(E,>(A,|(&(B,C),&(D,~(C)))))
                        duplicate = true;
                    }
                }

                if (!duplicate)
                {
                    temp.Propositions.Add(p);
                }
            }

            return temp;
        }

        public bool RemoveNegationDuplicate(MultiOr mo)
        {
            //return true;
            MultiOr temp = new MultiOr();
            bool add = true;

            foreach (Proposition p in mo.Propositions)
            {
                bool duplicate = false;
                foreach (Proposition tempProposition in temp.Propositions)
                {
                    var a = "¬(" + tempProposition.ToString() + ")";
                    var b = p.ToString();
                    var c = "¬(" + p.ToString() + ")";
                    var d = tempProposition.ToString();

                    if (a == b || c == d)
                    {
                        duplicate = true;
                        add = false;
                    }
                }

                if (!duplicate)
                {
                    temp.Propositions.Add(p);
                }
            }

            return add;
        }

        public bool RemoveTrue(MultiOr mo)
        {
            return mo.Propositions.Any(p => p.ToString() == "True");
        }

        public override string ToString()
        {
            //(A v B) ^ (C v D)
            string result = "";

            foreach (MultiOr proposition in this.Propositions)
            {
                result += "(";
                
                foreach (var p in proposition.Propositions)
                {
                    result += p.ToString() + " v ";
                }

                if (result.Length > 1)
                {
                    result = result.Remove(result.Length - 3);
                }

                result += ") ^ ";
            }

            if (result.Length > 0)
            {
                result = result.Remove(result.Length - 3);
            }

            if (result.Length == 3 || (result.Length == 6 && result.Contains("¬")) ||
                (result.Length == 7 && result.Contains("False")) ||
                (result.Length == 6 && result.Contains("True")))//(A) (False) (True) (~(A))
            {
                return result.Substring(1, result.Length - 2);
            }

            return result;
        }

        public override bool CalculateTruthTable()
        {
            List<bool> array = new List<bool>();
            foreach (MultiOr proposition in this.Propositions)
            {
                array.Add(proposition.CalculateTruthTable());
            }
            //0 1 1 0 0 1
            if (array.Count > 0)
            {
                bool result = array[0];
                for (int i = 1; i < array.Count; i++)
                {
                    result = result & array[i];
                }
                return result;
            }

            return false;
        }

        public override Proposition Nandify()
        {
            return this;
        }

        public override Proposition Copy()
        {
            MultiAnd newMultiAnd = new MultiAnd();
            foreach (MultiOr proposition in this.Propositions)
            {
                newMultiAnd.Propositions.Add((MultiOr)proposition.Copy());
            }

            return newMultiAnd;
        }

        public override MultiAnd Cnf()
        {
            return this;
        }

        public string GenerateCnfExpressionTree()
        {
            string outputString = "node" + this.Id + " [ label = \"" + this.Text + "\" ]\n";

            foreach (Proposition proposition in this.Propositions)
            {
                outputString += "node" + this.Id + " -- node" + proposition.Id + "\n";
                outputString += "node" + this.Id + " [ label = \"" + proposition.Text + "\" ]\n";
                foreach (var element in ((MultiOr)proposition).Propositions)
                {
                    outputString += "node" + proposition.Id + " -- node" + element.Id + "\n";
                    outputString += "node" + proposition.Id + " [ label = \"" + element.Text + "\" ]\n";
                    if (element.Text == "¬")
                    {
                        outputString += "node" + element.Id + " -- node" + element.LeftNode.Id + "\n";
                        outputString += "node" + element.Id + " [ label = \"" + element.LeftNode.Text + "\" ]\n";
                    }
                }
            }

            return outputString;
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