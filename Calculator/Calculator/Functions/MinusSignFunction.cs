namespace Calculator.Functions
{
    using System;

    public class MinusSignFunction : Node
    {
        public MinusSignFunction(Node leftNode, Node rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
            this.Label = "-";
        }

        public override double Calculate(double x)
        {
           return this.LeftNode.Calculate(x) - this.RightNode.Calculate(x);
        }

        public override Node Simplify()
        {
            Node n;
            if (this.LeftNode is NaturalNumberNode && this.RightNode is NaturalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) - Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            else if (this.LeftNode is RationalNumberNode && this.RightNode is RationalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) - Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            else
            {
                n = this;
                n.SetLeft(n.ReturnLeft().Simplify());
                n.SetRight(n.ReturnRight().Simplify());
                return n;
            }
        }

        public override string ToString()
        {
            return "(" + this.LeftNode + " - " + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node node = new MinusSignFunction(this.LeftNode, this.RightNode);
            node.SetLeft(this.LeftNode.ReturnDerivative());
            node.SetRight(this.RightNode.ReturnDerivative());

            return node;
        }
    }
}