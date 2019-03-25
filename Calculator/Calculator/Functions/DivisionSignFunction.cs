namespace Calculator.Functions
{
    using System;
    using static System.Double;
    
    public class DivisionSignFunction : Node
    {
        public DivisionSignFunction(Node leftNode, Node rightNode)
        {
            this.LeftNode = leftNode;
            this.RightNode = rightNode;
            this.Label = "/";
        }
        
        public override double Calculate(double x)
        {
            try
            {
                if (this.RightNode.Calculate(x) == 0)
                {
                    throw new DivideByZeroException();
                }
                return this.LeftNode.Calculate(x) / this.RightNode.Calculate(x);
            }
            catch
            {
                return this.LeftNode.Calculate(x) > 0 ? PositiveInfinity : NegativeInfinity;
            }
        }

        public override string ToString()
        {
            return "(" + this.LeftNode + " / " + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node leftNodeDerivative = this.LeftNode.ReturnDerivative();
            Node rightNodeDerivative = this.RightNode.ReturnDerivative();

            Node leftNodeCopy = this.LeftNode.copyFunction();
            Node rightNodeCopy = this.RightNode.copyFunction();

            Node divisionNode = new DivisionSignFunction(this.LeftNode, this.RightNode);
            Node minusNode = new MinusSignFunction(this.LeftNode, this.RightNode);
            Node firstMultiNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            Node secondMultiNode = new MultiplicationSignFunction(this.LeftNode, this.RightNode);

            firstMultiNode.SetLeft(leftNodeDerivative); 
            firstMultiNode.SetRight(rightNodeCopy);
               
            secondMultiNode.SetLeft(rightNodeDerivative);
            secondMultiNode.SetRight(leftNodeCopy);

            minusNode.SetLeft(firstMultiNode);
            minusNode.SetRight(secondMultiNode);

            Node powerNode = new PowerFunction(this.LeftNode, this.RightNode);
            powerNode.SetLeft(rightNodeCopy.copyFunction());
            powerNode.SetRight(new RationalNumberNode(2));

            divisionNode.SetLeft(minusNode);
            divisionNode.SetRight(powerNode);

            return divisionNode;
        }

        public override Node Simplify()
        {
            Node n;
            if (this.RightNode is NaturalNumberNode && this.RightNode.getText() == "1")
            {
                n = this.LeftNode;
                return n;
            }
            else if (this.RightNode is RationalNumberNode && this.RightNode.getText() == "1")
            {
                n = this.LeftNode;
                return n;
            }
            else if (this.LeftNode is NaturalNumberNode &&  this.RightNode is NaturalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) / Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            else if (this.LeftNode is RationalNumberNode && this.RightNode is RationalNumberNode)
            {
                var temp = Convert.ToDouble(this.LeftNode.getText()) / Convert.ToDouble(this.RightNode.getText());
                return new NaturalNumberNode(temp);
            }
            else
            {
                n = this;
                n.SetLeft(n.ReturnLeft().Simplify());
                n.SetRight(n.ReturnRight().Simplify());
                return n;
            }

            return n;
        }
    }
}