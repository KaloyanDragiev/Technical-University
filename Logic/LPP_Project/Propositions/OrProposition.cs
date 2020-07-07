using System;
using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public class OrProposition : Proposition
    {
        public OrProposition(Proposition leftNode, Proposition rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;

            this.Text = "V";
        }
        
        public override string ToString()
        {
            return "(" + this.LeftNode + " V " + this.RightNode + ")";
        }

        public override bool CalculateTruthTable()
        {
            return this.LeftNode.CalculateTruthTable() || this.RightNode.CalculateTruthTable();
        }

        public override Proposition Nandify()
        {
            //(A%A)%(B%B)
            return new NandProposition(new NandProposition(this.LeftNode.Nandify(), this.LeftNode.Nandify()), new NandProposition(this.RightNode.Nandify(), this.RightNode.Nandify()));
        }

        public override Proposition Copy()
        {
            return new OrProposition(this.LeftNode.Copy(), this.RightNode.Copy());
        }

        public override MultiAnd Cnf()
        {
            var leftCnf = this.LeftNode.Cnf();
            var rightCnf = this.RightNode.Cnf();

            MultiAnd multiAnd = leftCnf.Distribution(rightCnf);

            return multiAnd;
        }

        public override Proposition Simplify()
        {
            var leftCnf = this.LeftNode.Simplify();
            var rightCnf = this.RightNode.Simplify();

            return new OrProposition(leftCnf, rightCnf);
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            Proposition child1 = this.LeftNode.Tseitin(cnf);
            Proposition child2 = this.RightNode.Tseitin(cnf);

            MultiOr clause = new MultiOr();

            VariableProposition variable = new VariableProposition("");
            variable.AddVariable();//J
            //variables.Add(variable);
            Console.WriteLine(variable.Text + "<=> (" + child1.ToString() + " v " + child2.ToString() + ")");

            // J <-> (A v B)  ---- (~J v A v B) ^ (J v ~A) ^ (J v ~B)
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(child1);
            clause.Propositions.Add(child2);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(new NegationProposition(child1));
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(new NegationProposition(child2));
            cnf.AddProposition(clause);

            return variable;
        }
    }
}