namespace Calculator.Functions
{
    using System;

    public class ExponentialFunction : Node
    {
        public ExponentialFunction(Node rightNode)
        {
            this.RightNode = rightNode;
            this.Label = "exp";
        }

        public override double Calculate(double x)
        {
            var number = this.RightNode.Calculate(x);
            return Math.Exp(number);
        }

        public override Node Simplify()
        {
            var temp = Math.Exp(Convert.ToDouble(this.RightNode.getText()));
            return new NaturalNumberNode(temp);
        }

        public override string ToString()
        {
            return "Exp(" + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node rightDerivative = this.RightNode.ReturnDerivative();

            Node rightCopy = this.RightNode.copyFunction();

            Node nodeMultiplication = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            Node exponentialFunction = new ExponentialFunction(this.RightNode);
            exponentialFunction.SetRight(rightCopy);

            nodeMultiplication.SetLeft(exponentialFunction);
            nodeMultiplication.SetRight(rightDerivative);

            return nodeMultiplication;
        }
    }
}