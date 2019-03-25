namespace Calculator.Functions
{
    using System;

    public class SinFunction : Node
    {
        public SinFunction(Node rightNode)
        {
            this.RightNode = rightNode;
            this.Label = "sin";
        }

        public override double Calculate(double x)
        {
            return Math.Sin(this.RightNode.Calculate(x));
        }

        public override Node Simplify()
        {
            return this;
        }

        public override string ToString()
        {
            return "Sin(" + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node derivative = this.RightNode.ReturnDerivative();
            Node node = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            Node nodeCosine = new CosFunction(this.RightNode);
            
            nodeCosine.SetRight(this.RightNode.copyFunction());
            node.SetLeft(nodeCosine);
            node.SetRight(derivative);

            return node;
        }
    }
}