namespace Calculator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.CalculateDerivate = new System.Windows.Forms.Button();
            this.CalculateNewtonsDerivative = new System.Windows.Forms.Button();
            this.Increase = new System.Windows.Forms.Button();
            this.Decrease = new System.Windows.Forms.Button();
            this.Integral = new System.Windows.Forms.Button();
            this.Maclaurin = new System.Windows.Forms.Button();
            this.n1polynomial = new System.Windows.Forms.Button();
            this.pictureBoxDerivative = new System.Windows.Forms.PictureBox();
            this.ClearScreen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBoxIntegral = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxMaclaurin = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.formula = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDerivative)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntegral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaclaurin)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "Draw Function";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.DrawFunction_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(178, 114);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1419, 563);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGraph_Paint);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(178, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1257, 22);
            this.textBox1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(22, 725);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(391, 296);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // CalculateDerivate
            // 
            this.CalculateDerivate.Location = new System.Drawing.Point(12, 210);
            this.CalculateDerivate.Name = "CalculateDerivate";
            this.CalculateDerivate.Size = new System.Drawing.Size(149, 35);
            this.CalculateDerivate.TabIndex = 4;
            this.CalculateDerivate.Text = "Calculate Derivate";
            this.CalculateDerivate.UseVisualStyleBackColor = true;
            this.CalculateDerivate.Click += new System.EventHandler(this.CalculateDerivative_Click);
            // 
            // CalculateNewtonsDerivative
            // 
            this.CalculateNewtonsDerivative.Location = new System.Drawing.Point(12, 285);
            this.CalculateNewtonsDerivative.Name = "CalculateNewtonsDerivative";
            this.CalculateNewtonsDerivative.Size = new System.Drawing.Size(149, 35);
            this.CalculateNewtonsDerivative.TabIndex = 5;
            this.CalculateNewtonsDerivative.Text = "Newtons Derivative";
            this.CalculateNewtonsDerivative.UseVisualStyleBackColor = true;
            this.CalculateNewtonsDerivative.Click += new System.EventHandler(this.CalculateNewtonsDerivative_Click);
            // 
            // Increase
            // 
            this.Increase.Location = new System.Drawing.Point(22, 114);
            this.Increase.Name = "Increase";
            this.Increase.Size = new System.Drawing.Size(64, 39);
            this.Increase.TabIndex = 6;
            this.Increase.Text = "+";
            this.Increase.UseVisualStyleBackColor = true;
            this.Increase.Click += new System.EventHandler(this.Increase_Click);
            // 
            // Decrease
            // 
            this.Decrease.Location = new System.Drawing.Point(101, 114);
            this.Decrease.Name = "Decrease";
            this.Decrease.Size = new System.Drawing.Size(60, 39);
            this.Decrease.TabIndex = 7;
            this.Decrease.Text = "-";
            this.Decrease.UseVisualStyleBackColor = true;
            this.Decrease.Click += new System.EventHandler(this.Decrease_Click);
            // 
            // Integral
            // 
            this.Integral.Location = new System.Drawing.Point(12, 356);
            this.Integral.Name = "Integral";
            this.Integral.Size = new System.Drawing.Size(149, 35);
            this.Integral.TabIndex = 8;
            this.Integral.Text = "Integral";
            this.Integral.UseVisualStyleBackColor = true;
            this.Integral.Click += new System.EventHandler(this.Integral_Click);
            // 
            // Maclaurin
            // 
            this.Maclaurin.Location = new System.Drawing.Point(12, 423);
            this.Maclaurin.Name = "Maclaurin";
            this.Maclaurin.Size = new System.Drawing.Size(149, 35);
            this.Maclaurin.TabIndex = 9;
            this.Maclaurin.Text = "Maclaurin Series";
            this.Maclaurin.UseVisualStyleBackColor = true;
            this.Maclaurin.Click += new System.EventHandler(this.Maclaurin_Click);
            // 
            // n1polynomial
            // 
            this.n1polynomial.Location = new System.Drawing.Point(12, 487);
            this.n1polynomial.Name = "n1polynomial";
            this.n1polynomial.Size = new System.Drawing.Size(149, 35);
            this.n1polynomial.TabIndex = 10;
            this.n1polynomial.Text = "n+1 polynomial";
            this.n1polynomial.UseVisualStyleBackColor = true;
            this.n1polynomial.Click += new System.EventHandler(this.n1polynomial_Click);
            // 
            // pictureBoxDerivative
            // 
            this.pictureBoxDerivative.Location = new System.Drawing.Point(433, 725);
            this.pictureBoxDerivative.Name = "pictureBoxDerivative";
            this.pictureBoxDerivative.Size = new System.Drawing.Size(376, 296);
            this.pictureBoxDerivative.TabIndex = 11;
            this.pictureBoxDerivative.TabStop = false;
            // 
            // ClearScreen
            // 
            this.ClearScreen.Location = new System.Drawing.Point(1458, 12);
            this.ClearScreen.Name = "ClearScreen";
            this.ClearScreen.Size = new System.Drawing.Size(139, 35);
            this.ClearScreen.TabIndex = 13;
            this.ClearScreen.Text = "Clear Screen";
            this.ClearScreen.UseVisualStyleBackColor = true;
            this.ClearScreen.Click += new System.EventHandler(this.ClearScreen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Zoom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 689);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Function Graph and  N+1 polynomial";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(430, 689);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Derivative Graph";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "----------------------------------";
            // 
            // pictureBoxIntegral
            // 
            this.pictureBoxIntegral.Location = new System.Drawing.Point(830, 725);
            this.pictureBoxIntegral.Name = "pictureBoxIntegral";
            this.pictureBoxIntegral.Size = new System.Drawing.Size(376, 296);
            this.pictureBoxIntegral.TabIndex = 18;
            this.pictureBoxIntegral.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(827, 689);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Integral Graph";
            // 
            // pictureBoxMaclaurin
            // 
            this.pictureBoxMaclaurin.Location = new System.Drawing.Point(1221, 725);
            this.pictureBoxMaclaurin.Name = "pictureBoxMaclaurin";
            this.pictureBoxMaclaurin.Size = new System.Drawing.Size(376, 296);
            this.pictureBoxMaclaurin.TabIndex = 20;
            this.pictureBoxMaclaurin.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1218, 689);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 17);
            this.label6.TabIndex = 21;
            this.label6.Text = "Maclaurin Graph";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(827, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 17);
            this.label7.TabIndex = 22;
            this.label7.Text = "Function Graphic";
            // 
            // formula
            // 
            this.formula.AutoSize = true;
            this.formula.Location = new System.Drawing.Point(284, 70);
            this.formula.Name = "formula";
            this.formula.Size = new System.Drawing.Size(59, 17);
            this.formula.TabIndex = 23;
            this.formula.Text = "Formula";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1609, 1033);
            this.Controls.Add(this.formula);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBoxMaclaurin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBoxIntegral);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CalculateNewtonsDerivative);
            this.Controls.Add(this.CalculateDerivate);
            this.Controls.Add(this.ClearScreen);
            this.Controls.Add(this.pictureBoxDerivative);
            this.Controls.Add(this.n1polynomial);
            this.Controls.Add(this.Maclaurin);
            this.Controls.Add(this.Integral);
            this.Controls.Add(this.Decrease);
            this.Controls.Add(this.Increase);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Graph Calculator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDerivative)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIntegral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaclaurin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button CalculateDerivate;
        private System.Windows.Forms.Button CalculateNewtonsDerivative;
        private System.Windows.Forms.Button Increase;
        private System.Windows.Forms.Button Decrease;
        private System.Windows.Forms.Button Integral;
        private System.Windows.Forms.Button Maclaurin;
        private System.Windows.Forms.Button n1polynomial;
        private System.Windows.Forms.PictureBox pictureBoxDerivative;
        private System.Windows.Forms.Button ClearScreen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxIntegral;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxMaclaurin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label formula;
    }
}

