using System;
using System.Collections.Generic;
using System.Linq;

namespace LPP
{
    public class Node
    {
        private static int nextId = 0;
        private List<Proposition> propositions;
        private List<char> variables;

        public Node(List<Proposition> propositions, List<char> variables)
        {
            Id = nextId++;
            this.propositions = propositions;
            this.variables = variables;
            Closed = false;
        }

        public int Id { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public bool Closed { get; set; }

        private string ToInfixString()
        {
            string text;
            if (variables.Count > 0)
            {
                text = "[";
                foreach (char v in variables)
                {
                    text += v + ", ";
                }
                text = text.Substring(0, text.Length - 2) + "]\n {";
            }
            else
            {
                text = "{";
            }
            foreach (Proposition p in propositions)
            {
                text += p.ToInfixString() + ", ";
            }
            return text.Substring(0, text.Length - 2) + "}";
        }

        public string ToGraph()
        {
            string text = Id + "[label = \"" + @ToInfixString() + ((Closed) ? "\" color=\"red\"" : "\"") + " shape=\"box\"] ";
            if (Left != null)
            {
                text += Id + " -- " + Left.Id + " " + Left.ToGraph();
            }
            if (Right != null)
            {
                text += Id + " -- " + Right.Id + " " + Right.ToGraph(); ;
            }
            return text;
        }

        public void CheckClosed()
        {
            if (Left == null && Right == null)
            {
                foreach (Proposition p1 in propositions)
                {
                    if (p1 is NotOperator not)
                    {
                        foreach (Proposition p2 in propositions)
                        {
                            if (not.Left.ToInfixString() == p2.ToInfixString())
                            {
                                Closed = true;
                                return;
                            }
                        }
                    }
                    if (p1 is False)
                    {
                        Closed = true;
                        return;
                    }
                }
            }
            else
            {
                if (Left != null)
                {
                    Left.CheckClosed();
                    if (Left.Closed && Right == null)
                    {
                        Closed = true;
                        return;
                    }
                }
                if (Right != null)
                {
                    Right.CheckClosed();
                    if (Left.Closed && Right.Closed)
                    {
                        Closed = true;
                        return;
                    }
                }
            }
        }

        private bool CheckContains(List<Proposition> propositions, Proposition p)
        {
            foreach (Proposition prop in propositions)
            {
                if (prop.ToInfixString() == p.ToInfixString())
                {
                    return true;
                }
            }
            return false;
        }

        private void ReplaceVariable(Quantifier q, char v)
        {
            foreach (Predicate p in q.GetPredicates())
            {
                for (int j = 0; j < p.Variables.Count; j++)
                {
                    if (q.Variables.First() == p.Variables[j])
                    {
                        p.Variables[j] = v;
                    }
                }
            }
        }

        private bool GenerateLeftNode(int i, Proposition p, Proposition p1)
        {
            List<Proposition> propositionsLeft = new List<Proposition>(propositions)
            {
                [i] = p
            };
            if (p1 != null)
            {
                if (!CheckContains(propositionsLeft, p1))
                {
                    propositionsLeft.Add(p1);
                }
            }
            Left = new Node(propositionsLeft, variables);
            Left.CheckClosed();
            if (!Left.Closed)
            {
                Left.GenerateNodes();
            }
            return true;
        }

        private bool GenerateLeftRightNode(int i, Proposition p, Proposition p1)
        {
            List<Proposition> propositionsLeft = new List<Proposition>(propositions)
            {
                [i] = p
            };
            List<Proposition> propositionsRight = new List<Proposition>(propositions)
            {
                [i] = p1
            };
            Left = new Node(propositionsLeft, variables);
            Left.GenerateNodes();
            Right = new Node(propositionsRight, variables);
            Right.GenerateNodes();
            return true;
        }

        private bool GenerateNodeBiimplication(int i, Proposition p, Proposition p1, Proposition p2, Proposition p3)
        {
            List<Proposition> propositionsLeft = new List<Proposition>(propositions)
            {
                [i] = p
            };
            propositionsLeft.Add(p1);
            List<Proposition> propositionsRight = new List<Proposition>(propositions)
            {
                [i] = p2
            };          
            propositionsRight.Add(p3);
            Left = new Node(propositionsLeft, variables);
            Left.GenerateNodes();
            Right = new Node(propositionsRight, variables);
            Right.GenerateNodes();
            return true;
        }

        private bool GenerateNodeQuantifier(int i, Quantifier q, bool gamma)
        {
            if (Id > 1000 || variables.Count > 30)
            {
                throw new Exception("Infinite recursion. Not a tautology.");
            }
            Console.WriteLine(ToInfixString());
            List<Proposition> propositionsLeft = new List<Proposition>(propositions);
            List<char> newVariables = new List<char>(variables);
            if (gamma)
            {
                propositionsLeft.RemoveAt(i);
                if (q is ExistentialQuantifier)
                {
                    if (variables.Count > 0)
                    {                    
                        foreach (char v in variables)
                        {
                            Quantifier newQ = (Quantifier)q.Copy();
                            ReplaceVariable(newQ, v);
                            if (!CheckContains(propositionsLeft, new NotOperator(newQ.Left)))
                            {
                                propositionsLeft.Add(new NotOperator(newQ.Left));
                            }
                        }
                    }
                    else
                    {
                        propositionsLeft.Add(new NotOperator(((Quantifier)q.Copy()).Left));
                    }
                    propositionsLeft.Add(new NotOperator(q));
                }
                else
                {
                    if (variables.Count > 0)
                    {
                        foreach (char v in variables)
                        {
                            Quantifier newQ = (Quantifier)q.Copy();
                            ReplaceVariable(newQ, v);
                            if (!CheckContains(propositionsLeft, newQ.Left))
                            {
                                propositionsLeft.Add(newQ.Left);
                            }
                        }
                    }
                    else
                    {
                        propositionsLeft.Add(((Quantifier)q.Copy()).Left);
                    }
                    propositionsLeft.Add(q);
                }
            }
            else
            {
                char v = variables.Count > 0 ? (char)(variables[variables.Count - 1] + 1) : 'a';
                newVariables.Add(v);
                Quantifier newQ = (Quantifier)q.Copy();
                ReplaceVariable(newQ, v);
                if (q is UniversalQuantifier)
                {
                    propositionsLeft[i] = new NotOperator(newQ.Left);
                }
                else
                {
                    propositionsLeft[i] = newQ.Left;
                }
            }
            Left = new Node(propositionsLeft, newVariables);
            Left.GenerateNodes();
            return true;
        }

        public void GenerateNodes()
        {
            CheckClosed();
            if (Closed)
            {
                return;
            }
            if (MainRules())
            {
                return;
            }
            if (AlphaRules())
            {
                return;
            }
            if (DeltaRules())
            {
                return;
            }
            if (BetaRules())
            {
                return;
            }
            GammaRules();
        }

        private bool MainRules()
        {
            for (int i = 0; i < propositions.Count; i++)
            {
                if (propositions[i] is NotOperator notT && notT.Left is True t)
                {
                    return GenerateLeftNode(i, new False(t.Values.Count), null);
                }
                if (propositions[i] is NotOperator notF && notF.Left is False f)
                {
                    return GenerateLeftNode(i, new True(f.Values.Count), null);
                }
                if (propositions[i] is NotOperator not && not.Left is NotOperator doubleNot)
                {
                    return GenerateLeftNode(i, doubleNot.Left, null);
                }
            }
            return false;
        }

        private bool AlphaRules()
        {
            for (int i = 0; i < propositions.Count; i++)
            {
                if (propositions[i] is AndOperator and)
                {
                    return GenerateLeftNode(i, and.Left, and.Right);
                }
                if (propositions[i] is NotOperator not && not.Left is OrOperator notOr)
                {
                    return GenerateLeftNode(i, new NotOperator(notOr.Left), new NotOperator(notOr.Right));
                }
                if (propositions[i] is NotOperator notI && notI.Left is ImplicationOperator notImpl)
                {
                    return GenerateLeftNode(i, notImpl.Left, new NotOperator(notImpl.Right));
                }
                if (propositions[i] is NotOperator notN && notN.Left is NANDOperator notNand)
                {
                    return GenerateLeftNode(i, notNand.Left, notNand.Right);
                }
            }
            return false;
        }

        private bool DeltaRules()
        {
            for (int i = 0; i < propositions.Count; i++)
            {
                if (propositions[i] is ExistentialQuantifier exQuan)
                {
                    return GenerateNodeQuantifier(i, exQuan, false);
                }
                if (propositions[i] is NotOperator not && not.Left is UniversalQuantifier notUniQuan)
                {
                    return GenerateNodeQuantifier(i, notUniQuan, false);
                }
            }
            return false;
        }

        private bool BetaRules()
        {
            for (int i = 0; i < propositions.Count; i++)
            {
                if (propositions[i] is NotOperator not && not.Left is AndOperator notAnd)
                {
                    return GenerateLeftRightNode(i, new NotOperator(notAnd.Left), new NotOperator(notAnd.Right));
                }
                if (propositions[i] is OrOperator or)
                {
                    return GenerateLeftRightNode(i, or.Left, or.Right);
                }
                if (propositions[i] is ImplicationOperator impl)
                {
                    return GenerateLeftRightNode(i, new NotOperator(impl.Left), impl.Right);
                }
                if (propositions[i] is NANDOperator nand)
                {
                    return GenerateLeftRightNode(i, new NotOperator(nand.Left), new NotOperator(nand.Right));
                }
                if (propositions[i] is BiImplicationOperator biimpl)
                {
                    return GenerateNodeBiimplication(i, biimpl.Left, biimpl.Right, new NotOperator(biimpl.Left), new NotOperator(biimpl.Right));
                }
                if (propositions[i] is NotOperator notB && notB.Left is BiImplicationOperator notBi)
                {
                    return GenerateNodeBiimplication(i, notBi.Left, new NotOperator(notBi.Right), new NotOperator(notBi.Left), notBi.Right);
                }
            }
            return false;
        }

        private bool GammaRules()
        {
            for (int i = 0; i < propositions.Count; i++)
            {
                if (propositions[i] is UniversalQuantifier uniQuan)
                {
                    return GenerateNodeQuantifier(i, uniQuan, true);
                }
                if (propositions[i] is NotOperator not && not.Left is ExistentialQuantifier notExQuan)
                {
                   return GenerateNodeQuantifier(i, notExQuan, true);
                }  
            }
            return false;
        }
    }
}
