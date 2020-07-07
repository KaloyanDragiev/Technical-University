using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Proposition root;
        private List<Predicate> predicates;
        private Node rootNode;

        public MainWindow()
        {
            InitializeComponent();
        }       

        private void PrintTruthTable(List<char> result)
        {
            lbTruthTable.Items.Clear();
            lbSimpleTable.Items.Clear();
            string headings = "";           
            foreach (Predicate p in predicates)
            {
                headings += p.Letter + "\t";
            }
            headings += root.ToInfixString();
            lbTruthTable.Items.Add(headings);
            lbSimpleTable.Items.Add(headings);
            for (int i = 0; i < result.Count; i++)
            {
                string row = "";
                for (int j = 0; j < predicates.Count; j++)
                {
                    row += predicates[j].Values[i] + "\t";
                }
                row += result[i];
                lbTruthTable.Items.Add(row);              
            }
            lbHashes.Items.Add("Input hash code: " + PropositionManager.GetHash(result));           
        }

        private void PrintSimpleTruthTable(Dictionary<int, List<List<char>>> result)
        {
            foreach (int i in result.Keys)
            {
                if (result.ContainsKey(i))
                {
                    foreach (List<char> r in result[i])
                    {
                        if (r.Last() != '-')
                        {
                            string row = "";
                            foreach (char c in r)
                            {
                                row += c + "\t";
                            }
                            lbSimpleTable.Items.Add(row);
                        }
                        
                    }
                }
            }
        }

        private void btnParse_Click(object sender, RoutedEventArgs e)
        {
            lbHashes.Items.Clear();
            lbUnbound.Items.Clear();
            InputReader ir = new InputReader(tbInput.Text);
            try
            {                  
                root = ir.CheckInput();
                predicates = ir.GetPredicates();
                PropositionManager.PopulatePredicateValues(predicates);
                tbInfix.Text = root.ToInfixString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (ir.IsProposition)
            {
                List<char> result = root.GenerateTruthTable();
                PrintTruthTable(result);
                Proposition nandified = root.Nandify();
                tbNandify.Text = nandified.ToInfixString();
                lbHashes.Items.Add("Nandified hash code: " + PropositionManager.GetHash(nandified.GenerateTruthTable()));
                Proposition normalForm;
                if (result.Count > 2)
                {
                    normalForm = PropositionManager.BuildNormalForm(predicates, result);
                }
                else
                {
                    normalForm = root;
                }
                tbNormalForm.Text = normalForm.ToInfixString();
                lbHashes.Items.Add("Normal form hash code: " + PropositionManager.GetHash(normalForm.GenerateTruthTable()));
                normalForm = normalForm.Nandify();
                tbNormalNandify.Text = normalForm.ToInfixString();
                lbHashes.Items.Add("Nandified normal form hash code: " + PropositionManager.GetHash(normalForm.GenerateTruthTable()));
                PrintSimpleTruthTable(PropositionManager.SimplifyTruthTable(predicates, result));        
            }
            else
            {
                foreach (char c in ir.GetUnboundVariables())
                {
                    lbUnbound.Items.Add(c);
                }
            }
            GraphManager.Generate(root.ToGraph(), imgTree);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GraphManager.Clear(imgTree);
        }

        private void btnSubstitute_Click(object sender, RoutedEventArgs e)
        {
            PropositionManager.SubstituteVariable(predicates, Convert.ToChar(lbUnbound.SelectedItem), Convert.ToChar(tbSubstitute.Text));
            tbInfix.Text = root.ToInfixString();
            lbUnbound.Items.Add(Convert.ToChar(tbSubstitute.Text));
            GraphManager.Generate(root.ToGraph(), imgTree);
        }

        private void btnTableaux_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputReader ir = new InputReader(tbInput.Text);
                root = ir.CheckInput();
                rootNode = PropositionManager.SemanticTableaux(root);
                rootNode.CheckClosed();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            lblTautology.Content = "Is tautology? " + rootNode.Closed;
            btnShowTree.IsEnabled = true;           
        }

        private void btnShowTree_Click(object sender, RoutedEventArgs e)
        {
            if (rootNode != null)
            {
                GraphManager.Generate(rootNode.ToGraph(), imgTree);
            }         
        }
    }
}
