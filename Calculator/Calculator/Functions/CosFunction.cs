namespace Calculator.Functions
{
    using System;

    public class CosFunction : Node
    {
        public CosFunction(Node rightNode)
        {
            this.RightNode = rightNode;
            this.Label = "cos";
        }

        public override double Calculate(double x)
        {
            return Math.Cos(this.RightNode.Calculate(x));
        }

        public override Node Simplify()
        {
            return this;
        }

        public override string ToString()
        {
            return "Cos(" + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node derivative = this.RightNode.ReturnDerivative();

            Node node = new MultiplicationSignFunction(this.LeftNode , this.RightNode);
            Node nodeMinus = new MinusSignFunction(this.LeftNode, this.RightNode);
            Node nodeSine = new SinFunction(this.RightNode);

            nodeMinus.SetLeft(new RationalNumberNode(0));
            nodeSine.SetRight(this.RightNode.copyFunction());
            nodeMinus.SetRight(nodeSine);

            node.SetLeft(nodeMinus);
            node.SetRight(derivative);

            return node;
        }
    }
}