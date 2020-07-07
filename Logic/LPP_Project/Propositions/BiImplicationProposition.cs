using System;

namespace LPP_Project.Propositions
{
    using System.Collections.Generic;

    public class BiImplicationProposition : Proposition
    {
        public BiImplicationProposition(Proposition leftNode, Proposition rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;

            this.Text = "<=>";
        }

        public override string ToString()
        {
            return "(" +this.LeftNode + " <=> " + this.RightNode + ")";
        }

        public override bool CalculateTruthTable()
        {
            //p <-> q == p->q && q->p
            //~p V q  == p->q
            //~q V p  == q->p
            return (!this.LeftNode.CalculateTruthTable() || this.RightNode.CalculateTruthTable()) && 
                   (!this.RightNode.CalculateTruthTable() || this.LeftNode.CalculateTruthTable());
        }

        public override Proposition Nandify()
        {
            //(A%B)%(B%B) a->b
            //(b%a)%(a%a) b->a
            //((A%B)%(B%B) % (b%a)%(a%a) )%((b%a)%(a%a) % (A%B)%(B%B))  a->b && b->a
            // (((A % A)%(B % B))%((A % A)%(B % B)) %((A % A)%(B % B))%((A % A)%(B % B)) )%((A%B)%(A%B)%(A%B)%(A%B))

            Proposition lastAnd = new NandProposition(new NandProposition(this.LeftNode.Nandify(), this.RightNode.Nandify()), new NandProposition(this.LeftNode.Nandify(), this.RightNode.Nandify()));
            Proposition negation1 = new NandProposition(this.LeftNode.Nandify(), this.LeftNode.Nandify());
            Proposition negation2 = new NandProposition(this.RightNode.Nandify(), this.RightNode.Nandify());
            Proposition negationAndNegation = 
                new NandProposition(new NandProposition(negation1, negation2),
                new NandProposition(negation1, negation2));
            Proposition final = 
                new NandProposition(new NandProposition(negationAndNegation, negationAndNegation), 
                new NandProposition(lastAnd, lastAnd));
            return final;
        }

        public override Proposition Copy()
        {
            return new BiImplicationProposition(this.LeftNode.Copy(), this.RightNode.Copy());
        }

        public override MultiAnd Cnf()
        {
            //P <=> Q  =  (~P v Q) ^ (P v ~Q)

            var first = new NegationProposition(this.LeftNode).Cnf();
            var second = this.RightNode.Cnf();

            MultiOr multiOrFirst = first.JoinOr(second);//(~P v Q)

            var third = new NegationProposition(this.RightNode).Cnf();
            var fourth = this.LeftNode.Cnf();

            MultiOr multiOrSecond = third.JoinOr(fourth);//(P v ~Q)

            MultiAnd multiAnd = new MultiAnd();
            multiAnd.AddProposition(multiOrFirst);
            multiAnd.AddProposition(multiOrSecond);

            return multiAnd;
        }

        public override Proposition Simplify()
        {
            //P <=> Q  =  (~P v Q) ^ (P v ~Q)

            var negP = new NegationProposition(this.LeftNode.Simplify());
            var negQ = new NegationProposition(this.RightNode.Simplify());

            var firstOr = new OrProposition(negP, this.RightNode.Simplify());
            var secondOr = new OrProposition(negQ, this.LeftNode.Simplify());

            AndProposition and = new AndProposition(firstOr, secondOr);
            return and;
        }

        public override Proposition Tseitin(MultiAnd cnf)
        {
            Proposition child1 = this.LeftNode.Tseitin(cnf);
            Proposition child2 = this.RightNode.Tseitin(cnf);

            MultiOr clause = new MultiOr();

            VariableProposition variable = new VariableProposition("");//Create J And check form the variables list which is not yet used?
            variable.AddVariable();
            Console.WriteLine(variable.Text + "<=> (" + child1.ToString() + " <=> " + child2.ToString() + ")");

            // J <-> (A <-> B)  ---- (J v A v B) ^ (J v ~A v ~B) ^ (~J v A v ~B) ^ (~J v ~A v B)
            clause.Propositions.Add(variable);
            clause.Propositions.Add(child1);
            clause.Propositions.Add(child2);
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(variable);
            clause.Propositions.Add(new NegationProposition(child1));
            clause.Propositions.Add(new NegationProposition(child2));
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(child1);
            clause.Propositions.Add(new NegationProposition(child2));
            cnf.AddProposition(clause);

            clause = new MultiOr();
            clause.Propositions.Add(new NegationProposition(variable));
            clause.Propositions.Add(new NegationProposition(child1));
            clause.Propositions.Add(child2);
            cnf.AddProposition(clause);

            return variable;//return the cnf to the next?
        }
    }
}