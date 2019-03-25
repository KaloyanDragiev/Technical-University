namespace Calculator.Functions
{
    using System;

    public class MultiplicationSignFunction : Node
    {
        public MultiplicationSignFunction(Node leftNode, Node rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
            this.Label = "*";
        }

        public override double Calculate(double x)
        {
            return this.LeftNode.Calculate(x) * this.RightNode.Calculate(x);
        }

        public override Node Simplify()
        {
            Node n;
            if (this.LeftNode is RationalNumberNode && this.RightNode is RationalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) * Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            if (this.LeftNode is NaturalNumberNode && this.RightNode is NaturalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) * Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            else if ((this.RightNode is NaturalNumberNode && this.RightNode.getText() == "0") 
                     || (this.LeftNode is NaturalNumberNode && this.LeftNode.getText() == "0"))
            {
                return new NaturalNumberNode(0);
            }
            else if ((this.RightNode is RationalNumberNode && this.RightNode.getText() == "0")
                     || (this.LeftNode is RationalNumberNode && this.LeftNode.getText() == "0"))
            {
                return new NaturalNumberNode(0);
            }
            else if (this.LeftNode is NaturalNumberNode && this.LeftNode.getText() == "1")
            {
                n = this.RightNode;
                return n;
            }
            else if (this.LeftNode is RationalNumberNode && this.LeftNode.getText() == "1")
            {
                n = this.RightNode;
                return n;
            }
            else if ((this.RightNode is NaturalNumberNode && this.RightNode.getText() == "1"))
            {
                n = this.LeftNode;
                return n;
            }
            else if ((this.RightNode is RationalNumberNode && this.RightNode.getText() == "1"))
            {
                n = this.LeftNode;
                return n;
            }
            else if (this.LeftNode.getText() == "0" || this.RightNode.getText() == "0")
            {
                return new NaturalNumberNode(0);
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
            return "(" + this.LeftNode + " * " + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node leftDerivative = this.LeftNode.ReturnDerivative();
            Node rightDerivative = this.RightNode.ReturnDerivative();

            Node leftNode = this.LeftNode.copyFunction();
            Node rightNode = this.RightNode.copyFunction();

            Node node = new PlusSignFunction(this.LeftNode, this.RightNode);

            Node firstNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            firstNode.SetLeft(leftDerivative);
            firstNode.SetRight(rightNode);

            Node secondNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            secondNode.SetLeft(rightDerivative);
            secondNode.SetRight(leftNode);

            node.SetLeft(firstNode);
            node.SetRight(secondNode);

            return node;
        }
    }
}