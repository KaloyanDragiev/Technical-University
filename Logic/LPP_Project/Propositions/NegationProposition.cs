using System;
using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public class NegationProposition : Proposition
    {
        public NegationProposition(Proposition leftNode)
        {
            this.LeftNode = leftNode;

            this.Text = "¬";
        }
        
        public override string ToString()
        {
            return "¬" + "(" + this.LeftNode + ")";
        }

        public override bool CalculateTruthTable()
        {
            return !this.LeftNode.CalculateTruthTable();
        }

        public override Proposition Nandify()
        { 
            //~P = (P % P)
            return new NandProposition(this.LeftNode.Nandify(), this.LeftNode.Nandify());
        }

        public override Proposition Copy()
        {
            return new NegationProposition(this.LeftNode.Copy());
        }

        public override MultiAnd Cnf()
        {
            MultiOr multiOr = new MultiOr();
            MultiAnd multiAnd = new MultiAnd();

            if (this.LeftNode is NegationProposition)
            {
                return this.LeftNode.LeftNode.Cnf();
            }

            if (this.LeftNode is AndProposition)
            {
                var first = new NegationProposition(this.LeftNode.LeftNode).Cnf();
                var second = new NegationProposition(this.LeftNode.RightNode).Cnf();

                OrProposition or = new OrProposition(first, second);

                multiAnd = or.Cnf();
                
                return multiAnd;
            }

            //&(&(A,B),~(|(A,B)))
            if (this.LeftNode is OrProposition)
            {
                var first = new NegationProposition(this.LeftNode.LeftNode).Cnf();
                var second = new NegationProposition(this.LeftNode.RightNode).Cnf();

                multiAnd = (MultiAnd)first.JoinAnd(second);

                return multiAnd;
            }

            if (this.LeftNode is TrueProposition)
            {
                multiOr.Propositions.Add(new FalseProposition());
                
                multiAnd.Propositions.Add(multiOr);
                return multiAnd;
            }
            if (this.LeftNode is FalseProposition)
            {
                multiOr.Propositions.Add(new TrueProposition());
                
                multiAnd.Propositions.Add(multiOr);
                return multiAnd;
            }

            multiOr.Propositions.Add(this);
            multiAnd.Propositions.Add(multiOr);

            return multiAnd;
        }

        public override Proposition Simplify()
        {
            return new NegationProposition(this.LeftNode.Simplify());
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            Proposition child1 = this.LeftNode.Tseitin(cnf);

            MultiOr clause = new MultiOr();

            VariableProposition variable = new VariableProposition("");//провери дали вече я има и вземи това буква
            variable.AddVariable();//J

            Console.WriteLine(variable.Text + "<=> (~" + child1.ToString() + ")");

            // J <-> ~A  ---- (A v J) ^ (¬J v ¬A)
            clause.Propositions.Add(variable);
            clause.Propositions.Add(child1);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(new NegationProposition(child1));
            cnf.AddProposition(clause);

            return variable;
        }
    }
}