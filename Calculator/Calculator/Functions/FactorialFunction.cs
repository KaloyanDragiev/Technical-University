namespace Calculator.Functions
{
    using System;

    public class FactorialFunction : Node
    {
        public FactorialFunction(Node rightNode)
        {
            this.RightNode = rightNode;
            this.Label = "!";
        }

        public override double Calculate(double x)
        {
            return AdditionClass.Factorial((int) this.RightNode.Calculate(x));
        }

        public override Node Simplify()
        {
            var temp = AdditionClass.Factorial((int)Convert.ToDouble(this.RightNode.getText()));
            return new NaturalNumberNode(temp);
        }

        public override string ToString()
        {
            return this.RightNode + "!";
        }

        public string returnPrefix()
        {
            return "!(" + this.RightNode.getText() + ")";
        }

        public override Node ReturnDerivative()
        {
            return new RationalNumberNode(0);
        }
    }

    public static class AdditionClass
    {
        public static double Factorial(int calculate)
        {
            if (calculate <= 1)
                return 1;
            return calculate * Factorial(calculate - 1);
        }
    }
}