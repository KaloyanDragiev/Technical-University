using System.Collections.Generic;

namespace LPP_Project.Propositions
{
    public abstract class Proposition
    {
        private static int id;

        public int Id { get; set; }
        public string Text { get; set; }

        public Proposition RightNode;
        public Proposition LeftNode;
        public Proposition Root { get; private set; }

        protected Proposition()
        {
            id++;
            this.Id = id;
            this.Root = this;
        }

        public int CalculateRoot(bool[] variableValues)
        {
            int counter = 0;

            foreach (var variableFunction in ParseExpression.GetVariables())
            {
                variableFunction.Value = variableValues[counter];
                counter++;
            }

            //foreach (var variableFunction in VariableProposition.GeneratedList)
            //{
            //    variableFunction.Value = variableValues[counter];
            //    counter++;
            //}

            return this.Root.CalculateTruthTable() == false ? 0 : 1;
        }

        public string GenerateExpressionTree()
        {
            string outputString = "node" + this.Id + " [ label = \"" + this.Text + "\" ]\n";

            if (this.LeftNode != null)
            {
                outputString += "node" + this.Id + " -- node" + this.LeftNode.Id + "\n";
                outputString += this.LeftNode.GenerateExpressionTree();
            }
            if (this.RightNode != null)
            {
                outputString += "node" + this.Id + " -- node" + this.RightNode.Id + "\n";
                outputString += this.RightNode.GenerateExpressionTree();
            }

            return outputString;
        }
        
        public abstract bool CalculateTruthTable();

        public abstract Proposition Nandify();

        public abstract Proposition Copy();

        public abstract MultiAnd Cnf();

        public abstract Proposition Simplify();
        public abstract Proposition Tseitin(MultiAnd cnf);
    }
}