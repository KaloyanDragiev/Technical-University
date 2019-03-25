namespace Calculator.Functions
{
    public  class VariableNode : Node
    {
        public VariableNode()
        {
            this.Label = 'x'.ToString();
        }

        public override double Calculate(double x)
        {
            return x;
        }

        public override Node Simplify()
        {
            return this;
        }

        public override string ToString()
        {
            return this.Label;
        }

        public override Node ReturnDerivative()
        {
            return new RationalNumberNode(1);
        }
    }
}