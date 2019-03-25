namespace Calculator.Functions
{
    using static System.Double;

    public class IntegralFunction : Node
    {
        public double FromRange { get; private set; }
        public double ToRange { get; private set; }
        public IntegralFunction(Node node, double fromRange, double toRange) : base()
        {
            this.Label = "∫";

            if (fromRange > toRange)
            {
                this.FromRange = toRange;
                this.ToRange = fromRange;
                //LeftNode = PrefixParser.getFunction("-(0," + node.getText() + ")");
            }
            else
            {
                this.LeftNode = node;
                this.FromRange = fromRange;
                this.ToRange = toRange;
            }
        }

        public double GetFunctionY(double x)
        {
            return this.LeftNode.Calculate(x);
        }

        public override double Calculate(double x)
        {
            double sum = 0;

            for (double i = this.FromRange * x; i < this.ToRange * x; i++)
            {
                double adder = this.LeftNode.Calculate(i / x);
                if (IsInfinity(adder) || IsNaN(adder))
                {
                    continue;
                }
                sum += adder;
            }

            return sum / x;
        }

        public override Node Simplify()
        {
            return this;
        }

        public override Node ReturnDerivative()
        {
            return this.LeftNode;
        }
    }
}
