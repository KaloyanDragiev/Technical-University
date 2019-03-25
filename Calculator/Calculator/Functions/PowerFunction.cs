namespace Calculator.Functions
{
    using System;
    using static System.Double;

    public class PowerFunction : Node
    {
        public PowerFunction(Node leftNode, Node rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
            this.Label = "^";
        }

        public override double Calculate(double x)
        {
            if (x < 0 && this.LeftNode is VariableNode && this.RightNode is VariableNode)
            {
                return PositiveInfinity;
            }

            return Math.Pow(this.LeftNode.Calculate(x), this.RightNode.Calculate(x));
        }

        public override Node Simplify()
        {
            Node n;
            if (this.LeftNode is NaturalNumberNode && this.RightNode is NaturalNumberNode)
            {
                var temp = Math.Pow(Convert.ToDouble(this.LeftNode.getText()), Convert.ToDouble(this.RightNode.getText()));
                return new NaturalNumberNode(temp);
            }
            else if ((this.RightNode is NaturalNumberNode && this.RightNode.getText() != "0")
                     || (this.LeftNode is NaturalNumberNode && this.LeftNode.getText() == "0"))
            {
                return new NaturalNumberNode(0);
            }
            else if (this.LeftNode is NaturalNumberNode && this.LeftNode.getText() == "1")
            {
                n = this.LeftNode;
                return n;
            }
            else if ((this.RightNode is NaturalNumberNode && this.RightNode.getText() == "0"))
            {
                return new NaturalNumberNode(1);
            }
            else if (this.LeftNode.getText() == "0" || this.RightNode.getText() == "0")
            {
                return new NaturalNumberNode(1);
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
            return "(" + this.LeftNode + " ^ " + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node p1 = this.LeftNode.ReturnDerivative();
            Node p2 = this.RightNode.ReturnDerivative();

            Node leftNodeCopyFunction = this.LeftNode.copyFunction();
            Node rightNodeCopyFunction = this.RightNode.copyFunction();

            if (leftNodeCopyFunction.getText().IndexOf('x') > -1)
            {
                if (rightNodeCopyFunction.getText().IndexOf('x') > -1)
                {
                    return new RationalNumberNode(0);
                }
                Node node = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
                Node multiNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
                Node powerNode = new PowerFunction(this.LeftNode, this.RightNode);
                Node minusNode = new MinusSignFunction(this.LeftNode, this.RightNode);

                minusNode.SetLeft(rightNodeCopyFunction);
                minusNode.SetRight(new RationalNumberNode(1));

                powerNode.SetLeft(leftNodeCopyFunction);
                powerNode.SetRight(minusNode);

                multiNode.SetLeft(powerNode);
                multiNode.SetRight(rightNodeCopyFunction.copyFunction());

                node.SetLeft(multiNode);
                node.SetRight(p1);

                return node;
            }
            if (rightNodeCopyFunction.getText().IndexOf('x') > -1)
            {
                Node node = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
                Node multiNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
                Node logNode = new LogarithmFunction(this.RightNode);

                logNode.SetRight(leftNodeCopyFunction);

                Node powerNode = new PowerFunction(this.LeftNode, this.RightNode);
                powerNode.SetLeft(leftNodeCopyFunction);
                powerNode.SetRight(leftNodeCopyFunction.copyFunction());

                multiNode.SetLeft(logNode);
                multiNode.SetRight(powerNode);

                node.SetLeft(multiNode);
                node.SetLeft(p2);

                return node;
            }

            return new RationalNumberNode(0);
        }
    }
}