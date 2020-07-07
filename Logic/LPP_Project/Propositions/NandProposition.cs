using System;
using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public class NandProposition : Proposition
    {
        public NandProposition(Proposition leftNode, Proposition rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;

            this.Text = "%";
        }
        //¬(A⋀B)
        public override string ToString()
        {
            return this.Text + "(" + this.LeftNode + "," + this.RightNode + ")";
            //return  "~ (" + this.LeftNode + " ^ " + this.RightNode + ")";
            //"%(|(~(=(A,A)),B),>(%(B,0),B))
        }

        public override bool CalculateTruthTable()
        {
            return !(this.LeftNode.CalculateTruthTable() && this.RightNode.CalculateTruthTable());
        }

        public override Proposition Nandify()
        {
            return this;
            //return new NegationMultiAnd(new AndMultiAnd(this.LeftNode.Nandify(), this.RightNode.Nandify()));
        }

        public override Proposition Copy()
        {
            return new NandProposition(this.LeftNode.Copy(), this.RightNode.Copy());
        }

        public override MultiAnd Cnf()
        {
            MultiOr multiOr = new MultiOr();
            multiOr.Propositions.Add(this);

            MultiAnd multiAnd = new MultiAnd();
            multiAnd.AddProposition(multiOr);

            return multiAnd;
        }

        public override Proposition Simplify()
        {
            return this;
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            Proposition child1 = this.LeftNode.Tseitin(cnf);
            Proposition child2 = this.RightNode.Tseitin(cnf);

            MultiOr clause = new MultiOr();

            VariableProposition variable = new VariableProposition("");//Create J And check form the variables list which is not yet used?
            variable.AddVariable();//J
            Console.WriteLine(variable.Text + "<=> (" + child1.ToString() + " % " + child2.ToString() + ")");

            // J <-> (A % B)  ---- (~J v ~A v ~B) ^ (J v A) ^ (J v B)
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(new NegationProposition(child1));
            clause.Propositions.Add(new NegationProposition(child2));
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(child1);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(child2);
            cnf.AddProposition(clause);

            return variable;//return the cnf to the next?
        }
    }
}