namespace LPP_Project.SemanticTable
{
    using System;
    using Quantifiers;
    using Propositions;
    using System.Collections.Generic;

    public class Element
    {
        private static int nextId;
        private readonly List<Proposition> listOfPropositions;
        private readonly List<char> variables;

        public int Id { get; set; }
        public Element Left { get; set; }
        public Element Right { get; set; }
        public bool Closed { get; set; }

        public Element(List<Proposition> listOfPropositions, List<char> variables)
        {
            this.Id = nextId++;
            this.listOfPropositions = listOfPropositions;
            this.variables = variables;
            this.Closed = false;
        }
        
        public void CheckClosed()
        {
            if (this.Left == null && this.Right == null)
            {
                foreach (Proposition p1 in this.listOfPropositions)
                {
                    if (p1 is NegationProposition not)
                    {
                        foreach (Proposition p2 in this.listOfPropositions)
                        {
                            if (not.LeftNode.ToString() != p2.ToString()) continue;

                            this.Closed = true;
                            return;
                        }
                    }

                    if (!(p1 is FalseProposition)) continue;
                    this.Closed = true;
                    return;
                }
            }
            else
            {
                if (this.Left != null)
                {
                    this.Left.CheckClosed();
                    if (this.Left.Closed && this.Right == null)
                    {
                        this.Closed = true;
                        return;
                    }
                }
                if (this.Right != null)
                {
                    this.Right.CheckClosed();
                    Element left = this.Left;
                    if (left != null && (left.Closed && this.Right.Closed))
                    {
                        this.Closed = true;
                    }
                }
            }
        }

        private static bool CheckContains(List<Proposition> propositions, Proposition p)
        {
            foreach (Proposition prop in propositions)
            {
                if (prop.ToString() == p.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        private bool GenerateLeftNode(int i, Proposition p, Proposition p1)
        {
            //creates new propositionSet and sets the position of the new multiAnd
            List<Proposition> setOfPropositionsLeft = new List<Proposition>(this.listOfPropositions);
            setOfPropositionsLeft[i] = p;

            if (p1 != null)
            {
                if (!CheckContains(setOfPropositionsLeft, p1))
                {
                    setOfPropositionsLeft.Add(p1);
                }
            }

            this.Left = new Element(setOfPropositionsLeft, this.variables);
            this.Left.CheckClosed();
            if (!this.Left.Closed)
            {
                this.Left.GenerateNewNodes();
            }
            return true;
        }

        private bool GenerateLeftRightNode(int i, Proposition p, Proposition p1)
        {
            //creates new propositionSets and sets the position of the new multiAnd
            List<Proposition> setOfPropositionsLeft = new List<Proposition>(this.listOfPropositions);
            setOfPropositionsLeft[i] = p;
            List<Proposition> setOfPropositionsRight = new List<Proposition>(this.listOfPropositions);
            setOfPropositionsRight[i] = p1;

            this.Left = new Element(setOfPropositionsLeft, this.variables);
            this.Left.GenerateNewNodes();
            this.Right = new Element(setOfPropositionsRight, this.variables);
            this.Right.GenerateNewNodes();

            return true;
        }

        private bool GenerateNodeBiImplication(int i, Proposition p, Proposition p1, Proposition p2, Proposition p3)
        {
            //creates new propositionSets and sets the position of the new multiAnd
            List<Proposition> setOfPropositionsLeft = new List<Proposition>(this.listOfPropositions);
            setOfPropositionsLeft[i] = p;
            setOfPropositionsLeft.Add(p1);

            List<Proposition> setOfPropositionsRight = new List<Proposition>(this.listOfPropositions);
            setOfPropositionsRight[i] = p2;
            setOfPropositionsRight.Add(p3);

            this.Left = new Element(setOfPropositionsLeft, this.variables);
            this.Left.GenerateNewNodes();
            this.Right = new Element(setOfPropositionsRight, this.variables);
            this.Right.GenerateNewNodes();

            return true;
        }

        private bool GenerateNodeQuantifier(int i, Quantifier q, bool gamma)
        {
            if (this.Id > 1000 || this.variables.Count > 30)
            {
                throw new Exception("Infinite recursion. Not a tautology.");
            }
            Console.WriteLine(this.ToInfixString());
            List<Proposition> propositionsLeft = new List<Proposition>(this.listOfPropositions);
            List<char> newVariables = new List<char>(this.variables);
            if (gamma)
            {
                propositionsLeft.RemoveAt(i);
                if (q is ExistentialQuantifier)
                {
                    if (this.variables.Count > 0)
                    {
                        foreach (char v in this.variables)
                        {
                            Quantifier newQ = (Quantifier)q;
                            //this.ReplaceVariable(newQ, v);
                            if (!CheckContains(propositionsLeft, new NegationProposition(newQ.LeftNode)))
                            {
                                propositionsLeft.Add(new NegationProposition(newQ.LeftNode));
                            }
                        }
                    }
                    else
                    {
                        propositionsLeft.Add(new NegationProposition(((Quantifier)q).LeftNode));
                    }
                    propositionsLeft.Add(new NegationProposition(q));
                }
                else
                {
                    if (this.variables.Count > 0)
                    {
                        foreach (char v in this.variables)
                        {
                            Quantifier newQ = (Quantifier)q;
                            //this.ReplaceVariable(newQ, v);
                            if (!CheckContains(propositionsLeft, newQ.LeftNode))
                            {
                                propositionsLeft.Add(newQ.LeftNode);
                            }
                        }
                    }
                    else
                    {
                        propositionsLeft.Add(((Quantifier)q).LeftNode);
                    }
                    propositionsLeft.Add(q);
                }
            }
            else
            {
                char v = this.variables.Count > 0 ? (char)(this.variables[this.variables.Count - 1] + 1) : 'a';
                newVariables.Add(v);
                Quantifier newQ = (Quantifier)q;
                //this.ReplaceVariable(newQ, v);
                if (q is UniversalQuantifier)
                {
                    propositionsLeft[i] = new NegationProposition(newQ.LeftNode);
                }
                else
                {
                    propositionsLeft[i] = newQ.LeftNode;
                }
            }

            this.Left = new Element(propositionsLeft, newVariables);
            this.Left.GenerateNewNodes();
            return true;
        }

        public void GenerateNewNodes()
        {
            this.CheckClosed();
            if (this.Closed)
            {
                return;
            }
            if (this.MainRules())
            {
                return;
            }
            if (this.AlphaRules())
            {
                return;
            }
            if (this.DeltaRules())
            {
                return;
            }
            if (this.BetaRules())
            {
                return;
            }

            this.GammaRules();
        }

        private bool MainRules()
        {
            for (int i = 0; i < this.listOfPropositions.Count; i++)
            {
                if (this.listOfPropositions[i] is NegationProposition notT && notT.LeftNode is TrueProposition)
                {
                    //~(1)
                    return this.GenerateLeftNode(i, new FalseProposition(), null);
                }
                if (this.listOfPropositions[i] is NegationProposition notF && notF.LeftNode is FalseProposition)
                {
                    //~(0)
                    return this.GenerateLeftNode(i, new TrueProposition(), null);
                }
                if (this.listOfPropositions[i] is NegationProposition not && not.LeftNode is NegationProposition doubleNot)
                {
                    //~(~(A))
                    return this.GenerateLeftNode(i, doubleNot.LeftNode, null);
                }
            }
            return false;
        }

        private bool AlphaRules()
        {
            for (int i = 0; i < this.listOfPropositions.Count; i++)
            {
                if (this.listOfPropositions[i] is AndProposition and)
                {
                    return this.GenerateLeftNode(i, and.LeftNode, and.RightNode);
                }
                if (this.listOfPropositions[i] is NegationProposition not && not.LeftNode is OrProposition notOr)
                {
                    return this.GenerateLeftNode(i, new NegationProposition(notOr.LeftNode), new NegationProposition(notOr.RightNode));
                }
                if (this.listOfPropositions[i] is NegationProposition notI && notI.LeftNode is ImplicationProposition notImpl)
                {
                    return this.GenerateLeftNode(i, notImpl.LeftNode, new NegationProposition(notImpl.RightNode));
                }
                if (this.listOfPropositions[i] is NegationProposition notN && notN.LeftNode is NandProposition notNand)
                {
                    return this.GenerateLeftNode(i, notNand.LeftNode, notNand.RightNode);
                }
            }
            return false;
        }

        private bool BetaRules()
        {
            for (int i = 0; i < this.listOfPropositions.Count; i++)
            {
                if (this.listOfPropositions[i] is NegationProposition not && not.LeftNode is AndProposition notAnd)
                {
                    return this.GenerateLeftRightNode(i, new NegationProposition(notAnd.LeftNode), new NegationProposition(notAnd.RightNode));
                }
                if (this.listOfPropositions[i] is OrProposition or)
                {
                    return this.GenerateLeftRightNode(i, or.LeftNode, or.RightNode);
                }
                if (this.listOfPropositions[i] is ImplicationProposition impl)
                {
                    return this.GenerateLeftRightNode(i, new NegationProposition(impl.LeftNode), impl.RightNode);
                }
                if (this.listOfPropositions[i] is NandProposition nand)
                {
                    //%(A,B) => (A) , (B)
                    return this.GenerateLeftRightNode(i, new NegationProposition(nand.LeftNode), new NegationProposition(nand.RightNode));
                }
                if (this.listOfPropositions[i] is BiImplicationProposition biimpl)
                {
                    //=(A,B)  =>    (A, B) , (~A, ~B)
                    return this.GenerateNodeBiImplication(i, biimpl.LeftNode, biimpl.RightNode, new NegationProposition(biimpl.LeftNode), new NegationProposition(biimpl.RightNode));
                }
                if (this.listOfPropositions[i] is NegationProposition notB && notB.LeftNode is BiImplicationProposition notBi)
                {
                    //~(=(A,B))  =>    (A, ~B) , (~A, B)
                    return this.GenerateNodeBiImplication(i, notBi.LeftNode, new NegationProposition(notBi.RightNode), new NegationProposition(notBi.LeftNode), notBi.RightNode);
                }
            }
            return false;
        }

        private bool DeltaRules()
        {
            for (int i = 0; i < this.listOfPropositions.Count; i++)
            {
                if (this.listOfPropositions[i] is ExistentialQuantifier exQuan)
                {
                    return this.GenerateNodeQuantifier(i, exQuan, false);
                }
                if (this.listOfPropositions[i] is NegationProposition not && not.LeftNode is UniversalQuantifier notUniQuan)
                {
                    return this.GenerateNodeQuantifier(i, notUniQuan, false);
                }
            }
            return false;
        }

        private bool GammaRules()
        {
            for (int i = 0; i < this.listOfPropositions.Count; i++)
            {
                if (this.listOfPropositions[i] is UniversalQuantifier uniQuan)
                {
                    return this.GenerateNodeQuantifier(i, uniQuan, true);
                }
                if (this.listOfPropositions[i] is NegationProposition not && not.LeftNode is ExistentialQuantifier notExQuan)
                {
                    return this.GenerateNodeQuantifier(i, notExQuan, true);
                }
            }
            return false;
        }

        public string ToGraph()
        {
            string text = this.Id + "[label = \"" + this.ToInfixString() + ((this.Closed) ? "\" color=\"red\"" : "\"") + " shape=\"box\"] ";
            if (this.Left != null)
            {
                text += this.Id + " -- " + this.Left.Id + " " + this.Left.ToGraph();
            }
            if (this.Right != null)
            {
                text += this.Id + " -- " + this.Right.Id + " " + this.Right.ToGraph(); ;
            }
            return text;
        }

        private string ToInfixString()
        {
            string result;
            if (this.variables.Count > 0)
            {
                result = "[";
                foreach (char v in this.variables)
                {
                    result += v + ", ";
                }
                result = result.Substring(0, result.Length - 2) + "]\n {";
            }
            else
            {
                result = "{";
            }
            foreach (Proposition p in this.listOfPropositions)
            {
                result += p.ToString();
                result += ", ";
            }
            return result.Substring(0, result.Length - 2) + "}";
        }
    }
}
