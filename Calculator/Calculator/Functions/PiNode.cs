namespace Calculator.Functions
{
    using System;

    public class PiNode : Node
    {
        public PiNode()
        {
            this.Label = "3.14";
        }
        public override double Calculate(double x)
        {
            return Math.PI;
        }

        public override Node Simplify()
        {
            return this;
        }

        public override Node ReturnDerivative()
        {
            return new RationalNumberNode(0);
        }
    }
}