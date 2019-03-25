namespace Calculator.Functions
{
    public class DerivativeFunction : Node
    {
        private const double H = 0.0001d;

        public DerivativeFunction(Node node)
        {
            this.Label = "d/dx";
            this.LeftNode = node;
        }

        public override double Calculate(double x)
        {
            double fxh = this.LeftNode.Calculate(x + H);
            double fx = this.LeftNode.Calculate(x);

            return (fxh - fx) / H;
        }

        public override Node Simplify()
        {
            return this;
        }

        public override Node ReturnDerivative()
        {
            Node node = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            node.SetLeft(this.LeftNode.ReturnDerivative());
            node.SetRight(this.LeftNode);

            return node;
        }
    }
}
