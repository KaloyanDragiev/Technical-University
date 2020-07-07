using System.Collections.Generic;

namespace LPP_Project
{
    using System;
    using System.Linq;
    using Propositions;
    using SemanticTable;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public int Counter { get; set; }
        public Form1()
        {
            this.InitializeComponent();
            this.Counter = 200;
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.textBox7.Text = "";
            this.textBox8.Text = "";
            this.textBox9.Text = "";
            this.Tautology.Text = @"Is tautology? :";
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox3.Items.Clear();
            ParseExpression.ClearListOfVariables();
            VariableProposition.ClearListOfVariables();
            VariableProposition.Counter = 0;

            string expression = this.textBox1.Text.Trim().Replace(" ", string.Empty);

            Proposition proposition;
                
            if (expression.Contains("["))
            {
                proposition = ShorterFormat.ConvertFromShortFormat(expression);
            }
            else
            {
                proposition = ParseExpression.Parse(ref expression);
            }
            TseitinTransformation.Transform(proposition);

            //DrawBinaryTree.DrawTree(proposition, this.pictureBox1);

            Element node = SemanticTableaux.CreateSemanticTableaux(proposition);
            node.CheckClosed();
            if (node.Closed)
            {
                this.Tautology.BackColor = Color.Green;
                this.Tautology.Text += @"True";
            }
            else
            {
                this.Tautology.BackColor = Color.Red;
                this.Tautology.Text += @"False";
            }
            //DrawBinaryTree.DrawTreeSemanticTable(node.ToGraph(), this.pictureBox2);
            if (!ParseExpression.IsProposition) return;

            MultiAnd tseitin = new MultiAnd();//=(&(~(A),B),|(>(C,D),E))
            proposition.Tseitin(tseitin); //return list of newly creatred variables

            Console.WriteLine(ShorterFormat.ConvertToShortFormat(tseitin.ToString()));

            //Proposition multiAndNandify = proposition.Nandify();
            Proposition simplifyProposition = proposition.Simplify();//Simplify =>, <=>
            MultiAnd multiAndCnf = simplifyProposition.Cnf();//  ~(=(A,B)) >(=(A,A),A)

            DavidPuttnam.Solve(multiAndCnf, ParseExpression.GetVariables());//solve with DavisPutman

            //DrawBinaryTree.DrawTree(multiAndCnf, this.pictureBox2);
            TruthTable multiTable = new TruthTable(multiAndCnf);
            string hashCodeMulti = multiTable.GetHashcode(multiTable);

            this.textBox2.Text = proposition.ToString();//infix
            //this.textBox7.Text = multiAndNandify.ToString();//nandify

            this.textBox9.Text = multiAndCnf.ToString();
            this.textBox9.Text = ShorterFormat.ConvertToShortFormat(multiAndCnf.ToString());//Cnf
            
            this.textBox3.Text = string.Join(", ", ParseExpression.GetVariables());//Display variables
            string variablesSpace = string.Join(" ", ParseExpression.GetVariables());//Display variables names
            this.listBox1.Items.Add(variablesSpace + " Whole");
            this.listBox2.Items.Add(variablesSpace + " Whole");

            TruthTable table = new TruthTable(proposition);//Create table
            string hashCode = table.GetHashcode(table);//Get hashcode and calculate result and display
            this.textBox4.Text = hashCode;

            for (int i = 0; i < table.Rows; i++)//Print the TruthTable and display the result
            {
                this.listBox1.Items.Add(string.Join(" ", table.GetTable()[i].Value) + "     " + table.GetTable()[i].CalculatedValue);
            }

            TruthTable simplifiedTruthTable = table.SimplifyMultipleTimes(table);//Simplify the original table
            string hashCodeSimplified = table.GetHashcode(simplifiedTruthTable);
            string solvable = "0";

            foreach (var row in simplifiedTruthTable.GetTable())//Display simplified table
            {
                this.listBox2.Items.Add(string.Join(" ", row.Value) + "     " + row.CalculatedValue);
                if (row.CalculatedValue == "1")
                {
                    solvable = row.CalculatedValue;
                }
            }

            Proposition multiAndDnf = table.DisjunctiveNormalForm(table.GetTable());
            Proposition multiAndDnfs = table.DisjunctiveNormalFormSimplified(simplifiedTruthTable.GetTable());
            //Proposition multiAndDnfsNandify = multiAndDnfs.Nandify();

            this.textBox5.Text = multiAndDnf.ToString();//Disjunctive normal form for table
            this.textBox6.Text = multiAndDnfs.ToString();//Disjunctive normal form for simplified table
            //this.textBox8.Text = multiAndDnfsNandify.ToString();//Nandify the simplified table

            TruthTable newTableDnf = new TruthTable(multiAndDnf);//Creates new table and checks hash codes...
            string hashCodeDnf = newTableDnf.GetHashcode(newTableDnf);

            if (hashCode == hashCodeDnf)
            {
                this.listBox3.Items.Add(@"TruthTable:  " + hashCode);
                this.listBox3.Items.Add(@"DNF:  " + hashCodeDnf);
            }

            TruthTable newTableDnfSimplified = new TruthTable(multiAndDnfs);//Creates new table for simplify
            string hashCodeDnfs = newTableDnfSimplified.GetHashcode(newTableDnfSimplified);//simplify the new table and check hashcode

            if (hashCode == hashCodeDnfs)
                this.listBox3.Items.Add(@"DNFS:  " + hashCodeDnfs);

            //TruthTable nandifyTable = new TruthTable(multiAndNandify);
            //string hashCodeNandify = nandifyTable.GetHashcode(nandifyTable);

            //if (hashCode == hashCodeNandify)
            //    this.listBox3.Items.Add(@"NandifyNormal:  " + hashCodeNandify);

            //TruthTable nandifySimplifyTable = new TruthTable(multiAndDnfsNandify);//simplify the new table and check hashcode
            //string hashCodeSimplifyNandify = nandifySimplifyTable.GetHashcode(nandifySimplifyTable);

            //if (hashCode == hashCodeSimplifyNandify)
            //   this.listBox3.Items.Add(@"Nandify DNFS:  " + hashCodeSimplifyNandify);

            this.listBox3.Items.Add(@"Cnf:  " + hashCodeMulti);
            this.listBox3.Items.Add(@"Simplify:  " + hashCodeSimplified);

            this.listBox3.Items.Add(@"DavisPutman:  ");
            List<bool> aa = new List<bool>();
            foreach (var variablesValue in DavidPuttnam.VariablesValues)
            {
                this.listBox3.Items.Add(variablesValue.Key + " " + variablesValue.Value);
                aa.Add(variablesValue.Value == 1 ? true : false);
            }

            if (hashCodeMulti != hashCode)
            {
                MessageBox.Show("Error CNF");
            }

            string calculated = proposition.CalculateRoot(aa.ToArray()).ToString();
            this.listBox3.Items.Add(calculated);
            this.listBox3.Items.Add("");

            if (solvable != calculated)
            {
                MessageBox.Show("Error DAVIS");
            }

            this.listBox3.Items.Add(@"Tseitin and then DavisPutman:  ");
            DavidPuttnam.Solve(tseitin, ParseExpression.GetAllVariables());
            
            aa = new List<bool>();
            foreach (var variablesValue in DavidPuttnam.VariablesValues)
            {
                this.listBox3.Items.Add(variablesValue.Key + " " + variablesValue.Value);
                aa.Add(variablesValue.Value == 1 ? true : false);
            } 

            calculated = proposition.CalculateRoot(aa.ToArray()).ToString();
            this.listBox3.Items.Add(calculated);

            if (solvable != calculated)
            {
                MessageBox.Show("Error Tseitin with DAVIS");
            }

            this.listBox3.Items.Add("Tseitin: " + ShorterFormat.ConvertToShortFormat(tseitin.ToString()));

            //For running 200 times uncomment *

            this.Counter--;
            if (this.Counter > 0)
            {
                CreateRandom_Click(this, new EventArgs());
            }
        }
        
        private void CreateRandom_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = ParseExpression.CreateRandomProposition();
            this.Parse_Click(this, new EventArgs());
        }
    }
}