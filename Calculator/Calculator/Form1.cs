namespace Calculator
{
    using Draw;
    using System;
    using Functions;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private readonly Tree tree;
        private readonly Parser parser;
        private readonly DrawingGraph drawingGraph;
        private DrawBinaryTree drawingBinaryTree;

        private readonly IntegralForm integralForm;
        private readonly MaclaurinForm maclaurinForm;

        public Form1()
        {
            this.InitializeComponent();
            this.drawingGraph = new DrawingGraph(this.pictureBox);
            this.parser = new Parser();
            this.tree = new Tree();

            this.integralForm = new IntegralForm();
            this.integralForm.onAllDataInputed += this.SetIntegral;

            this.maclaurinForm = new MaclaurinForm();
            this.maclaurinForm.onAllDataInputed += this.SetMaclaurin;
        }

        private void DrawFunction_Click(object sender, EventArgs e)
        {
            try
            {
                Node node = this.parser.ParseExpression(this.textBox1.Text);
                this.tree.setRoot(node);
                this.drawingGraph.SetTree(this.tree);
                this.drawingBinaryTree = new DrawBinaryTree(this.tree, this.pictureBox2);
                if (tree.Root != null)
                {
                    this.formula.Text = "Current Formula:\n" + tree.Root.ToString();
                }
                this.pictureBox.Invalidate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void DrawGraph_Paint(object sender, PaintEventArgs e)
        {
            this.drawingGraph.Draw(e.Graphics);
        }

        private void CalculateDerivative_Click(object sender, EventArgs e)
        {
            try
            {
                Node node = this.parser.ParseExpression(this.textBox1.Text.Replace('p','3'));
                //I had some troubles with PI thats why i did it this way... :D 
                this.tree.setRoot(node);
                this.tree.setDerivative();
                this.tree.DisplayDerivative();
                this.drawingGraph.SetTree(this.tree);
                this.drawingBinaryTree = new DrawBinaryTree(this.tree, this.pictureBoxDerivative);

                this.pictureBox.Invalidate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CalculateNewtonsDerivative_Click(object sender, EventArgs e)
        {
            try
            {
                Node node = this.parser.ParseExpression(this.textBox1.Text);
                this.tree.setRoot(node);
                this.tree.DisplayNewtonDerivative();
                this.drawingGraph.SetTree(this.tree);
                this.drawingBinaryTree = new DrawBinaryTree(this.tree, this.pictureBoxDerivative);

                this.pictureBox.Invalidate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Increase_Click(object sender, EventArgs e)
        {
            this.drawingGraph.IncreaseScale(2f);
            this.pictureBox.Invalidate();
        }

        private void Decrease_Click(object sender, EventArgs e)
        {
            this.drawingGraph.IncreaseScale(0.5f);
            this.pictureBox.Invalidate();
        }

        private void Integral_Click(object sender, EventArgs e)
        {
            this.integralForm.SetFormula(this.textBox1.Text);
            this.integralForm.Toogle();
        }

        private void SetIntegral()
        {
            try
            {
                Node node = this.parser.ParseExpression(this.textBox1.Text);
                this.tree.setRoot(node);
                this.tree.DisplayDerivative();
                this.drawingGraph.SetTree(this.tree);
                this.drawingBinaryTree = new DrawBinaryTree(this.tree, this.pictureBoxIntegral);

                this.tree.setIntegral(this.integralForm.FromRange, this.integralForm.ToRange);
                this.pictureBox.Invalidate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Maclaurin_Click(object sender, EventArgs e)
        {
            this.maclaurinForm.SetFormula(this.textBox1.Text);
            this.maclaurinForm.Toogle();
        }
        
        private void SetMaclaurin()
        {
            try
            {
                //Node node = this.parser.ParseExpression(this.textBox1.Text);
                Node node = ParserMc.getFunction(this.textBox1.Text);
                this.tree.setRoot(new MaclaurinFunction(node.Simplify(), this.maclaurinForm.N));
                this.tree.DisplayDerivative();
                this.drawingGraph.SetTree(this.tree);
                this.drawingBinaryTree = new DrawBinaryTree(this.tree, this.pictureBoxMaclaurin);

                this.pictureBox.Invalidate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void n1polynomial_Click(object sender, EventArgs e)
        {
            Tree tree2 = new Tree(this.pictureBox2);
            this.drawingGraph.SetTree(tree2);

            ((Button)sender).BackColor = ((Button)sender).BackColor == Color.BurlyWood ? SystemColors.Control : Color.BurlyWood;
            this.drawingGraph.ChangeNPlusOne();

            this.pictureBox.Invalidate();
        }

        private void ClearScreen_Click(object sender, EventArgs e)
        {
            this.DrawGraph_Paint(sender, (PaintEventArgs)e);
        }
        
    }
}
