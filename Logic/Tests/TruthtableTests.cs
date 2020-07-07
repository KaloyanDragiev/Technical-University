namespace Tests
{
    using LPP_Project;
    using LPP_Project.Propositions;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TruthtableTests
    {
        [TestMethod]
        public void TestFalseValues()
        {
            string testInput = "0";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(FalseProposition));
            Assert.AreEqual(false, ((FalseProposition)root).CalculateTruthTable());
        }

        [TestMethod]
        public void TestVariableValues()
        {
            string testInput = "&(P,Q)";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            List<VariableProposition> variables = ParseExpression.GetVariables();
            Assert.AreEqual(2, variables.Count);
            Assert.AreEqual("0", table.GetTable()[0].Value[0]);
            Assert.AreEqual("0", table.GetTable()[0].Value[1]);
            Assert.AreEqual("0", table.GetTable()[1].Value[0]);
            Assert.AreEqual("1", table.GetTable()[1].Value[1]);
            Assert.AreEqual("1", table.GetTable()[2].Value[0]);
            Assert.AreEqual("0", table.GetTable()[2].Value[1]);
            Assert.AreEqual("1", table.GetTable()[3].Value[0]);
            Assert.AreEqual("1", table.GetTable()[3].Value[1]);
        }

        [TestMethod]
        public void TestResult()
        {
            string testInput = "&(P,Q)";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            List<VariableProposition> variables = ParseExpression.GetVariables();
            Assert.AreEqual(2, variables.Count);
            Assert.AreEqual("0", table.GetTable()[0].CalculatedValue);
            Assert.AreEqual("0", table.GetTable()[1].CalculatedValue);
            Assert.AreEqual("0", table.GetTable()[2].CalculatedValue);
            Assert.AreEqual("1", table.GetTable()[3].CalculatedValue);
        }

        [TestMethod]
        public void TestSimplification()
        {
            string testInput = "&(P,Q)";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            TruthTable simplifiedTruthTable = table.SimplifyMultipleTimes(table);
            string hashCodeSimplified = table.GetHashcode(simplifiedTruthTable);
            Assert.AreEqual(3, simplifiedTruthTable.GetTable().Count);
            Assert.AreEqual("*", simplifiedTruthTable.GetTable()[0].Value[1]);
            Assert.AreEqual("*", simplifiedTruthTable.GetTable()[1].Value[0]);
        }

        [TestMethod]
        public void TestNormalForm()
        {
            string testInput = "|(P,|(Q,R))";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            Proposition normalForm = table.DisjunctiveNormalForm(table.GetTable());
            Assert.IsInstanceOfType(normalForm, typeof(OrProposition));
            Assert.IsInstanceOfType((normalForm).RightNode, typeof(AndProposition));
        }
        
        [TestMethod]
        public void TestNandify()
        {
            string testInput = "|(P,|(Q,R))";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            Proposition nandified = root.Nandify();
            Assert.IsInstanceOfType(nandified, typeof(NandProposition));
            Assert.IsInstanceOfType(((NandProposition)nandified).LeftNode, typeof(NandProposition));
        }
        
        [TestMethod]
        public void TestHashCodes()
        {
            string testInput = "|(P,|(Q,R))";
            Proposition root = ParseExpression.Parse(ref testInput);
            root.SetRoot(root);
            TruthTable table = new TruthTable(root);
            string hashCode = table.GetHashcode(table);
            Proposition nandified = root.Nandify();
            Proposition normalForm = table.DisjunctiveNormalForm(table.GetTable());
            Proposition normalNandified = normalForm.Nandify();
            nandified.SetRoot(nandified);
            TruthTable newTableNand = new TruthTable(nandified);
            string hashNandified = newTableNand.GetHashcode(newTableNand);
            normalForm.SetRoot(normalForm);
            TruthTable newTableDnf = new TruthTable(normalForm);
            string hashNormalForm = newTableDnf.GetHashcode(newTableDnf);
            normalNandified.SetRoot(normalNandified);
            TruthTable newTableDnfN = new TruthTable(normalNandified);
            string hashNormalNandified = newTableDnf.GetHashcode(newTableDnfN);

            Assert.AreEqual(hashCode, hashNandified);
            Assert.AreEqual(hashNandified, hashNormalForm);
            Assert.AreEqual(hashNormalForm, hashNormalNandified);
        }
    }
}
