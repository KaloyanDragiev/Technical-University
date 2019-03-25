namespace Calculator
{
    using Functions;
    using System.Windows.Forms;
    
    public class Tree
    {
        private bool showIntegral;
        private bool showDerivative;
        private bool showNewtonDerivative;

        private Node root;
        private Node derivative;
        private PictureBox pictureBox;
        private IntegralFunction integral;

        public Node Root { get { return this.root; } }
        public Node Derivative { get { return this.derivative; } }
        public PictureBox PictureBox { get { return this.pictureBox; } }
        public IntegralFunction Integral { get { return this.integral; } }

        public Tree()
        {
            this.root = null;
        }

        public Tree(PictureBox displayStuff)
        {
            this.root = null;
            this.pictureBox = displayStuff;
        }

        public void setRoot(Node node)
        {
            this.root = node;
            this.derivative = null;
        }

        public void setDerivative()
        {
            this.showIntegral = true;
            this.derivative = this.root.ReturnDerivative().Simplify();
        }

        public void setIntegral(double from, double to)
        {
            this.integral = new IntegralFunction(this.Root, from, to);
        }

        public void DisplayDerivative()
        {
            this.showDerivative = !this.showDerivative;
            if (this.root != null && this.showDerivative)
            {
                this.setDerivative();
            }
            if (this.root != null && this.showIntegral)
            {
                this.setDerivative();
            }
        }

        public void DisplayNewtonDerivative()
        {
            this.showNewtonDerivative = !this.showNewtonDerivative;
            if (this.root != null && this.showNewtonDerivative)
            {
                this.derivative = new DerivativeFunction(this.root);
            }
        }
    }
}
