namespace Calculator.Draw
{
    using System;
    using Functions;
    using System.Linq;
    using System.Drawing;
    using static System.Double;
    using System.Windows.Forms;
    using System.Globalization;
    using System.Collections.Generic;

    public class DrawingGraph
    {
        private bool nPlusOne;
        private float tileSize;
        private bool displayNumbers;

        private Tree tree;

        private readonly PictureBox pictureBox;
        private List<PointF> points;

        private int x = 0;
        private int y = 0;

        public void DisplayNumbers(bool numbers)
        {
            this.displayNumbers = numbers;
        }

        public void MoveGraph(int x, int y)
        {
            this.x += x;
            this.y += y;
        }

        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }

        public float TileSize
        {
            get => this.tileSize;
            private set
            {
                if (value <= 1)
                {
                    this.tileSize = 1;
                }
                else
                {
                    this.tileSize = value;
                }
            }
        }

        public void IncreaseScale(float f)
        {
            this.TileSize *= f;
        }

        public void SetTree(Tree tree)
        {
            this.tree = tree;
        }

        private PointF TransformCoords(float ex, float ey)
        {
            float coefficient = this.Coefficient();
            ex = -(this.pictureBox.Width / 2 - ex) - this.x;
            ey = (this.pictureBox.Height / 2 - ey) + this.y;
            return new PointF(ex / coefficient, ey / coefficient);
        }

        public void ChangeNPlusOne()
        {
            this.nPlusOne = !this.nPlusOne;
            if (!this.nPlusOne)
            {
                this.points = new List<PointF>();
            }
        }

        public DrawingGraph(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            this.tileSize = 50;
            this.points = new List<PointF>();

            this.displayNumbers = true;
            this.nPlusOne = false;

            this.pictureBox.MouseClick += (sender, e) => {
                if (this.nPlusOne)
                {
                    this.points.Add(this.TransformCoords(e.X, e.Y));
                    this.pictureBox.Invalidate();
                }
            };
        }

        private int Coefficient()
        {
            int coefficient = (int) this.tileSize;

            if (coefficient == 0)
                return 1;

            return coefficient;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.White, 0, 0, this.pictureBox.Width, this.pictureBox.Height);
            graphics.TranslateTransform(this.pictureBox.Width / 2 + this.x, this.pictureBox.Height / 2 + this.y);

            int ts = (int) this.tileSize;

            if (ts != 0)
            {
                int w = this.pictureBox.Width + Math.Abs(2 * this.x); 
                int h = this.pictureBox.Height + Math.Abs(2 * this.y);
                for (int i = -(h / ts) / 2; i < (h / ts) / 2 + 1; i++)
                {
                    graphics.DrawLine(new Pen(Color.FromArgb(30, 0, 0, 0)), -w / 2, i * ts, w / 2, i * ts);
                }
                for (int j = -(w / ts) / 2; j < (w / ts) / 2 + 1; j++)
                {
                    graphics.DrawLine(new Pen(Color.FromArgb(30, 0, 0, 0)), j * ts, -h / 2, j * ts, h / 2);
                }

                graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), 0, -this.y - this.pictureBox.Height / 2, 0, this.pictureBox.Height / 2 - this.y);
                graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), -this.x - this.pictureBox.Width / 2, 0, -this.x + this.pictureBox.Width / 2, 0);     

                if (this.tree != null && this.tree.Root != null)
                {
                    if (this.tree.Integral != null)
                    {
                        this.DrawFunction(graphics, this.tree.Derivative, Pens.BlueViolet);
                        this.DrawIntegralFunction(graphics, this.tree.Integral, Brushes.GreenYellow);
                    }

                    if (this.tree.Derivative != null)
                    {
                        this.DrawFunction(graphics, this.tree.Derivative , Pens.BlueViolet);
                    }

                    this.DrawFunction(graphics, this.tree.Root, Pens.Blue);
                }

                if (this.displayNumbers)
                {
                    for (int i = -(h / ts) / 2; i < (h / ts) / 2 + 1; i++)
                    {
                        graphics.DrawString(-i + "", SystemFonts.DefaultFont, Brushes.Black, 0, i * ts);
                    }
                    for (int j = -(w / ts) / 2; j < (w / ts) / 2 + 1; j++)
                    {
                        graphics.DrawString(j + "", SystemFonts.DefaultFont, Brushes.Black, j * ts, 0);
                    }
                }

                foreach (PointF p in this.points)
                {
                    float coefficient = this.Coefficient();
                    graphics.FillRectangle(Brushes.Chocolate, p.X * coefficient - 2, -p.Y * coefficient - 2, 4, 4);
                }

                if (this.points.Count >= 2 && this.nPlusOne) {
                    this.DrawNplusOne(graphics);}
            }
        }

        public void DrawFunction(Graphics graphics, Node functionNode, Pen pen)
        {
            int coefficient = this.Coefficient();
            float[] points = new float[2];
            double xval = (((-this.x) - this.pictureBox.Width / 2 - 1) / (coefficient + 0f));

            try { points[0] = (float)functionNode.Calculate(xval) * coefficient; }
            catch { }

            for (int i = (int)(-this.x) - this.pictureBox.Width / 2; i < -this.x + this.pictureBox.Width / 2 + 3; i++)
            {
                try
                {
                    points[1] = points[0];
                    xval = (i / (coefficient + 0f));
                    float f2 = (float)functionNode.Calculate(xval) * coefficient;
                    points[0] = f2;
                    if (IsInfinity(f2) || IsNaN(f2)) { continue; }
                    graphics.DrawLine(pen, i, -points[0], i - 1, -points[1]);
                }
                catch (DivideByZeroException) { continue; }
                catch { continue; }
            }
        }
        
        public void DrawIntegralFunction(Graphics graphics, IntegralFunction integralFunction, Brush brush)
        {
            int coefficient = this.Coefficient();
            string res = integralFunction.Calculate(coefficient).ToString(CultureInfo.InvariantCulture);

            for (double i = integralFunction.FromRange * coefficient; i <= integralFunction.ToRange * coefficient; i++)
            {
                double result = integralFunction.GetFunctionY(i / coefficient) * coefficient;
                if (IsInfinity(result) || IsNaN(result)) { continue; }
                graphics.FillRectangle(-result < 0 ? brush : Brushes.Green, (float)i, -result < 0 ? -(float)result : 0, 1, (float)(Math.Abs(result)));
            }

            graphics.DrawString(res, SystemFonts.DefaultFont, Brushes.DarkGoldenrod, 
                (float)((integralFunction.FromRange + (integralFunction.ToRange - integralFunction.FromRange) / 2.0d) * coefficient) - 3, -20);
        }
        
        private void DrawNplusOne(Graphics graphics)
        {
            this.points = this.points.OrderBy(p => p.X).ToList();
            double[] xes = new double[this.points.Count];
            double[] yes = new double[this.points.Count];
            int i = 0;
            foreach (PointF p in this.points)
            {
                xes[i] = p.X;
                if (i > 0 && xes[i - 1] == xes[i])
                {
                    MessageBox.Show("You cannot have a function with 2 different x values for a same y value!");
                    this.ChangeNPlusOne();
                    return;
                }
                yes[i] = p.Y;
                i++;
            }
            double[,] mat = new double[xes.Length, yes.Length];
            for (int m = 0; m < xes.Length; m++)
            {
                for (int k = 0; k < yes.Length; k++)
                {
                    double val = Math.Pow(xes[m], yes.Length - 1 - k);
                    mat[m, k] = val;
                }
            }
            Matrix matrix = new Matrix(mat);
            double[] results = matrix.coeffOfPolynomial(yes);
            string formula = "0";
            int counter = results.Length - 1;

            foreach (double d in results)
            {
                formula = "+(*(r(" + d + "),^(x," + counter + "))," + formula + ")";
                counter--;
            }

            Parser parser = new Parser();
            var function = parser.ParseExpression(formula);
            this.DrawFunction(graphics, function, Pens.Chocolate);
            
            var drawBinaryTree = new DrawBinaryTree(function, this.tree.PictureBox);
        }
    }
}
