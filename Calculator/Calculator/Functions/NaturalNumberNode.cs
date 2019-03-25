namespace Calculator.Functions
{
    using System.Globalization;

    public class NaturalNumberNode : Node
    {
        private readonly int numberValue;

        public NaturalNumberNode(double number)
        {
            this.numberValue = (int)number;
            this.Label = number.ToString(CultureInfo.InvariantCulture);
        }

        public override double Calculate(double x)
        {
            return this.numberValue;
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
            return new RationalNumberNode(0);
        }
    }
}