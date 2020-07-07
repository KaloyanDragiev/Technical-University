using LPP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LLPTests
{
    [TestClass]
    public class PropositionManagementTests
    {
        [TestMethod]
        public void TestFalseValues()
        {
            string testInput = "0";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(False));
            Assert.AreEqual(false, ((False)root).Values.Contains('1'));
        }
        
        [TestMethod]
        public void TestPredicateValues()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.PopulatePredicateValues(predicates);
            Assert.AreEqual(4, predicates[0].Values.Count);
            Assert.AreEqual('0', predicates[0].Values[0]);
            Assert.AreEqual('1', predicates[0].Values[3]);
        }

        [TestMethod]
        public void TestResult()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.PopulatePredicateValues(predicates);
            List<char> result = root.GenerateTruthTable();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual('0', result[0]);
            Assert.AreEqual('1', result[3]);
        }

        [TestMethod]
        public void TestSimplification()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.PopulatePredicateValues(predicates);
            List<char> result = root.GenerateTruthTable();
            Dictionary<int, List<List<char>>> simplified = PropositionManager.SimplifyTruthTable(predicates, result);
            Assert.AreEqual(3, simplified.Keys.Count);
            Assert.AreEqual('-', simplified[0][0][2]);
        }

        [TestMethod]
        public void TestNormalForm()
        {
            string testInput = "|(P,|(Q,R))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.PopulatePredicateValues(predicates);
            List<char> result = root.GenerateTruthTable();
            Proposition normalForm = PropositionManager.BuildNormalForm(predicates, result);
            Assert.IsInstanceOfType(normalForm, typeof(OrOperator));
            Assert.IsInstanceOfType(((OrOperator)normalForm).Left, typeof(AndOperator));
        }

        [TestMethod]
        public void TestNandify()
        {
            string testInput = "|(P,|(Q,R))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Proposition nandified = root.Nandify();
            Assert.IsInstanceOfType(nandified, typeof(NANDOperator));
            Assert.IsInstanceOfType(((NANDOperator)nandified).Left, typeof(NANDOperator));
        }

        [TestMethod]
        public void TestHashCodes()
        {
            string testInput = "|(P,|(Q,R))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.PopulatePredicateValues(predicates);
            List<char> result = root.GenerateTruthTable();           
            Proposition nandified = root.Nandify();
            Proposition normalForm = PropositionManager.BuildNormalForm(predicates, result);
            Proposition normalNandified = normalForm.Nandify();
            string hash = PropositionManager.GetHash(result);
            string hashNandified = PropositionManager.GetHash(nandified.GenerateTruthTable());          
            string hashNormalForm = PropositionManager.GetHash(normalForm.GenerateTruthTable());
            string hashNormalNandified = PropositionManager.GetHash(normalNandified.GenerateTruthTable());
            Assert.AreEqual(hash, hashNandified);
            Assert.AreEqual(hashNandified, hashNormalForm);
            Assert.AreEqual(hashNormalForm, hashNormalNandified);
        }

        [TestMethod]
        public void TestSubstitute()
        {
            string testInput = "!x.(P(x,y))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            PropositionManager.SubstituteVariable(predicates, 'y', 'z');
            Assert.AreEqual('z', predicates[0].Variables[1]);
        }
    }
}
