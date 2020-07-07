using LPP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LLPTests
{
    [TestClass]
    public class InputParseTests
    {
        [TestMethod]
        public void TestNoInput()
        {
            string testInput = "";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.AreEqual(root, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Mismatched parentheses")]
        public void TestInvalidInputBrackets()
        {
            string testInput = "(";
            InputReader ir = new InputReader(testInput);
            ir.CheckInput();
        }

        [TestMethod]
        public void TestTrue()
        {
            string testInput = "1";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(True));
        }

        [TestMethod]
        public void TestValidProposition()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(AndOperator));
        }

        [TestMethod]
        public void TestToInfix()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(AndOperator));
            Assert.AreEqual("(P/\\Q)", root.ToInfixString());
        }

        [TestMethod]
        public void TestToGraph()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(AndOperator));
            Assert.AreEqual(true, root.ToGraph().Contains("label"));
        }

        [TestMethod]
        public void TestLeftRightPropositions()
        {
            string testInput = "&(P,>(Q,|(R,S)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(AndOperator));
            Assert.IsInstanceOfType(((AndOperator)root).Left, typeof(Predicate));
            Assert.IsInstanceOfType(((AndOperator)root).Right, typeof(ImplicationOperator));
        }

        [TestMethod]
        public void TestPredicateNumber()
        {
            string testInput = "&(P,>(Q,|(R,S)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            Assert.AreEqual(predicates.Count, 4);
        }

        [TestMethod]
        public void TestUniquePredicates()
        {
            string testInput = "&(P,>(Q,|(P,Q)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<Predicate> predicates = ir.GetPredicates();
            Assert.AreEqual(2, predicates.Count);
        }

        [TestMethod]
        public void TestSimpleQuantifier()
        {
            string testInput = "!x.(P)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(ExistentialQuantifier));
            Assert.AreEqual(false, ir.IsProposition);
        }

        [TestMethod]
        public void TestIsPropositionFormula()
        {
            string testInput = "&(P,>(Q,|(R,S)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.AreEqual(true, ir.IsProposition);
        }

        [TestMethod]
        public void TestNotQuantifier()
        {
            string testInput = "~(@x.(P(x)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(NotOperator));
            Assert.IsInstanceOfType(((NotOperator)root).Left, typeof(UniversalQuantifier));
        }

        [TestMethod]
        public void TestQuantifierUnBoundVariables()
        {
            string testInput = "!x,y.(P(x,z,w))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            List<char> boundVariables = ir.GetUnboundVariables();
            Assert.AreEqual(2, boundVariables.Count);
        }

        [TestMethod]
        public void TestPredicateVariables()
        {
            string testInput = "!x,y.(P(x,z,w))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Assert.IsInstanceOfType(root, typeof(ExistentialQuantifier));
            Assert.IsInstanceOfType(((ExistentialQuantifier)root).Left, typeof(Predicate));
            List<char> variables = ((Predicate)((ExistentialQuantifier)root).Left).Variables;
            Assert.AreEqual(3, variables.Count);
        }
    }
}
