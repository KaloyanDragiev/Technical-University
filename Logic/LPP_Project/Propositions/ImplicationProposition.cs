using System;

namespace LPP_Project.Propositions
{
    using System.Collections.Generic;

    public class ImplicationProposition : Proposition
    {
        public ImplicationProposition(Proposition leftNode, Proposition rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;

            this.Text = "=>";
        }

        public override string ToString()
        {
            return  "(" + this.LeftNode + " => " + this.RightNode + ")";
        }

        public override bool CalculateTruthTable()
        {
            //~p V q  == p->q
            return !this.LeftNode.CalculateTruthTable() || this.RightNode.CalculateTruthTable();
        }

        public override Proposition Nandify()
        {
            //A%(B%B)
            return new NandProposition(this.LeftNode.Nandify(), new NandProposition(this.RightNode.Nandify(), this.RightNode.Nandify()));
        }

        public override Proposition Copy()
        {
            return new ImplicationProposition(this.LeftNode.Copy(), this.RightNode.Copy());
        }

        public override MultiAnd Cnf()
        {
            //P => Q  (~P v Q)

            var leftCnf = new NegationProposition(this.LeftNode).Cnf();
            var rightCnf = this.RightNode.Cnf();

            if (this.LeftNode is AndProposition || this.RightNode is AndProposition ||
                rightCnf.Propositions.Count > 1 || leftCnf.Propositions.Count > 1)
            {
                MultiAnd multiAnd = leftCnf.Distribution(rightCnf);//(~P v (Q ^ C))

                return multiAnd;
            }
            else
            {
                MultiOr multiOr = leftCnf.JoinOr(rightCnf);//(~P v Q)
                MultiAnd multiAnd = new MultiAnd();
                multiAnd.AddProposition(multiOr);
                return multiAnd;
            }
        }

        public override Proposition Simplify()
        {
            //P => Q  (~P v Q)

            var negP = new NegationProposition(this.LeftNode.Simplify());

            var firstOr = new OrProposition(negP, this.RightNode.Simplify());

            return firstOr;
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            Proposition child1 = this.LeftNode.Tseitin(cnf);
            Proposition child2 = this.RightNode.Tseitin(cnf);

            MultiOr clause = new MultiOr();

            VariableProposition variable = new VariableProposition("");//Create J And check form the variables list which is not yet used?
            variable.AddVariable();

            Console.WriteLine(variable.Text + "<=> (" + child1.ToString() + " => " + child2.ToString() + ")");

            // J <-> (A -> B)  ---- (~J v ~A v B) ^ (J v A) ^ (J v ~B)
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(new NegationProposition(child1));
            clause.Propositions.Add(child2);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(child1);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(new NegationProposition(child2));
            cnf.AddProposition(clause);

            return variable;//return the cnf to the next?
        }
    }
}