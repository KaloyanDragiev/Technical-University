namespace Calculator.Functions
{
    using System;

    public class LogarithmFunction : Node
    {
        public LogarithmFunction(Node rightNode)
        {
            this.RightNode = rightNode;
            this.Label = "ln";
        }

        public override double Calculate(double x)
        {
            return Math.Log(this.RightNode.Calculate(x)); ;
        }

        public override Node Simplify()
        {
            this.RightNode.SetRight(this.RightNode.Simplify()) ;
            return this;
        }

        public override string ToString()
        {
            return "ln(" + this.RightNode + ")";
        }

        public override Node ReturnDerivative()
        {
            Node derivative = this.RightNode.ReturnDerivative();
            Node nodeRight = this.RightNode.copyFunction();

            Node node = new MultiplicationSignFunction(this.LeftNode, this.RightNode);
            Node nodeDivision = new DivisionSignFunction(this.LeftNode, this.RightNode);
                
            nodeDivision.SetLeft(new RationalNumberNode(1));
            nodeDivision.SetRight(nodeRight);

            node.SetLeft(nodeDivision);
            node.SetRight(derivative);

            return node;
        }
    }
}