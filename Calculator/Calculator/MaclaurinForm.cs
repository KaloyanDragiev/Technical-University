namespace Calculator
{
    using System;
    using System.Windows.Forms;

    public partial class MaclaurinForm : Form
    {
        private string formula;
        private bool showed;

        public int N { get; private set; }

        public delegate void AllDataInputed();
        public event AllDataInputed onAllDataInputed;

        public MaclaurinForm()
        {
            this.InitializeComponent();
            this.showed = false;
            this.onAllDataInputed += this.Toogle;
        }

        public void SetFormula(string formula)
        {
            this.formula = formula;
        }

        public void Toogle()
        {
            if (!this.showed)
            {
                this.label2.Text = this.formula;
                this.Show();
            }
            else
            {
                this.Hide();
                this.label2.Text = "";
                this.textBox1.Text = "";
            }

            this.showed = !this.showed;
        }


        private void Calculate_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "" || this.textBox1.Text == "inf")
            {
                this.N = 0;
            }
            else
            {
                this.N = Convert.ToInt32(this.textBox1.Text);
            }

            this.onAllDataInputed?.Invoke();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Toogle();
        }
    }
}
