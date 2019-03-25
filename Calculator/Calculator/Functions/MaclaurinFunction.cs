namespace Calculator.Functions
{
    public class MaclaurinFunction : Node
    {
        private int numberN;

        public MaclaurinFunction(Node functionNode, int numberN)
        {
            this.Label = "McLaurin";
            this.LeftNode = functionNode;
            this.numberN = numberN;

            this.RightNode = new RationalNumberNode(0);
             Node derivative = this.LeftNode;

             for(int i=0;i< numberN + 1;i++) {
                Node add = new PlusSignFunction(LeftNode,RightNode);
                Node power = new PowerFunction(LeftNode, RightNode);
                Node mult = new MultiplicationSignFunction(LeftNode, RightNode);
                Node div = new DivisionSignFunction(LeftNode, RightNode);
                Node fac = new FactorialFunction(RightNode);

                fac.SetLeft(new RationalNumberNode(i));

                power.SetLeft(new VariableNode());
                power.SetRight(new RationalNumberNode(i));

                mult.SetLeft(new RationalNumberNode((float)derivative.Calculate(0)));
                mult.SetRight(power);

                div.SetLeft(mult);
                div.SetRight(fac);

                add.SetLeft(this.RightNode.copyFunction());
                add.SetRight(div);

                this.RightNode = add;

                 derivative = derivative.ReturnDerivative();
             }
            
        }
        public override double Calculate(double x)
        {
            return this.RightNode.Calculate(x);
        }

        public override Node Simplify()
        {
            return this;
        }

        public override Node ReturnDerivative()
        {
            return this.RightNode.ReturnDerivative();
        }
    }
}
