namespace Calculator
{
    using System;
    using System.Windows.Forms;

    public partial class IntegralForm : Form
    {
        private double fromRange;
        private double toRange;
        private string formula;

        public double FromRange => this.fromRange;
        public double ToRange => this.toRange;
        private bool showed;

        public delegate void AllDataInputed();
        public event AllDataInputed onAllDataInputed;

        public IntegralForm()
        {
            this.InitializeComponent();
            this.showed = false;
            this.onAllDataInputed += this.Toogle;
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "" || this.textBox1.Text == "inf")
            {
                this.toRange = 0;
            }
            else
            {
                this.toRange = Convert.ToDouble(this.textBox1.Text);
            }

            if (this.textBox2.Text == "" || this.textBox2.Text == "inf")
            {
                this.fromRange = 0;
            }
            else
            {
                this.fromRange = Convert.ToDouble(this.textBox2.Text);
            }

            this.onAllDataInputed?.Invoke();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Toogle();
        }

        public void SetFormula(string formula)
        {
            this.formula = formula;
        }

        public void Toogle()
        {
            if (!this.showed)
            {
                this.label2.Text = this.formula + " dx";
                this.Show();
            }
            else
            {
                this.Hide();
                this.label2.Text = "";
                this.textBox1.Text = "";
                this.textBox2.Text = "";
            }

            this.showed = !this.showed;
        }
    }
}
