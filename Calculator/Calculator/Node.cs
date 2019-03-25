namespace Calculator
{
    using Functions;

    public abstract class Node
    {
        protected Node LeftNode;
        protected Node RightNode;

        private static int Id = 1;

        public int ID { get; }
        public string Label { get; protected set; }

        protected Node()
        {
            this.ID = Id;
            Id++;
        }

        public void SetRight(Node node)
        {
            this.RightNode = node;
        }
        public void SetLeft(Node node)
        {
            this.LeftNode = node;
        }

        public Node ReturnRight()
        {
            return this.RightNode;
        }
        public Node ReturnLeft()
        {
            return this.LeftNode;
        }

        public abstract Node ReturnDerivative();

        public abstract double Calculate(double x);

        public string GenerateBinaryTree()
        {
            string binaryTree = "node" + this.ID + " [ label = \"" + this.Label + "\" ]\n";
            if (this.LeftNode != null)
            {
                binaryTree += "node" + this.ID + " -- node" + this.LeftNode.ID + "\n";
                binaryTree += this.LeftNode.GenerateBinaryTree();
            }
            if (this.RightNode != null)
            {
                binaryTree += "node" + this.ID + " -- node" + this.RightNode.ID + "\n";
                binaryTree += this.RightNode.GenerateBinaryTree();
            }
            return binaryTree;
        }

        public abstract Node Simplify();

        public Node copyFunction()
        {
            Parser parser = new Parser();
            return parser.ParseExpression(((this.getText()).Replace("sin", "s").Replace("cos", "c").Replace("ln", "l").Replace("exp", "e")));
        }

        public string getText()
        {
            if (this is VariableNode || this is NaturalNumberNode)
            {
                return this.Label;
            }
            else if (this is RationalNumberNode)
            {
                return ((RationalNumberNode) this).Calculate(0) + "";
            }
            else
            {
                if (this is MaclaurinFunction)
                {
                    return this.RightNode.getText();
                }

                if (this is DerivativeFunction || this is IntegralFunction)
                {
                    return this.LeftNode.getText();
                }

                return this.Label + "(" +
                       (this.LeftNode == null
                           ? ""
                           : (this.LeftNode.Label == "!" ? this.LeftNode.getText() : this.LeftNode.getText() + ",")) +
                       (this.RightNode == null ? "" : this.RightNode.getText()) + ")";

            }
        }
    }
}
