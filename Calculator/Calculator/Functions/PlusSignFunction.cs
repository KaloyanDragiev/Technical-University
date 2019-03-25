namespace Calculator.Functions
{
    using System;

    public class PlusSignFunction : Node
    {
        public PlusSignFunction(Node leftNode, Node rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
            this.Label = "+";
        }

        public override double Calculate(double x)
        {
            return (this.LeftNode?.Calculate(x) ?? 0) + (this.RightNode?.Calculate(x) ?? 0);
        }

        public override Node Simplify()
        {
            Node n;
            if (this.LeftNode is RationalNumberNode && this.RightNode is RationalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) + Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            if (this.LeftNode is NaturalNumberNode && this.RightNode is NaturalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) + Convert.ToDouble(this.RightNode.getText());
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
            return "(" + this.LeftNode + " + " + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node leftNode = this.LeftNode.ReturnDerivative();
            Node rightNode = this.RightNode.ReturnDerivative(); 

            Node node = new PlusSignFunction(this.LeftNode, this.RightNode);
            node.SetLeft(leftNode);
            node.SetRight(rightNode);

            return node;
        }
    }
}
