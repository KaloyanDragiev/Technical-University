namespace Tests
{
    using System;
    using LPP_Project;
    using LPP_Project.Quantifiers;
    using LPP_Project.Propositions;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParseExpressionTests
    {
        [TestMethod]
        public void TestNoInput()
        {
            string testInput = "";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.AreEqual(root, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "The given expression contains invalid symbols or missing operand!")]
        public void TestInvalidExpression()
        {
            string testInput = @"(erer)";
            Proposition root = ParseExpression.Parse(ref testInput);
        }

        [TestMethod]
        public void TestTrue()
        {
            string testInput = "1";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(TrueProposition));
        }

        [TestMethod]
        public void TestValidProposition()
        {
            string testInput = "&(P,Q)";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(AndProposition));
        }

        [TestMethod]
        public void TestToInfix()
        {
            string testInput = "&(P,Q)";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(AndProposition));
            Assert.AreEqual("(P ⋀ Q)", root.ToString());
        }

        [TestMethod]
        public void TestLeftRightPropositions()
        {
            string testInput = "&(P,>(Q,|(R,S)))";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(AndProposition));
            Assert.IsInstanceOfType(((AndProposition)root).LeftNode, typeof(VariableProposition));
            Assert.IsInstanceOfType(((AndProposition)root).RightNode, typeof(ImplicationProposition));
        }

        [TestMethod]
        public void TestPredicateNumber()
        {
            string testInput = "&(~(|(P,Q)),>(=(Q,R),=(S,T)))";
            Proposition root = ParseExpression.Parse(ref testInput);
            List<VariableProposition> variables = ParseExpression.GetVariables();
            Assert.AreEqual(variables.Count, 5);
        }

        [TestMethod]
        public void TestUniquePredicates()
        {
            string testInput = "~(~(~(&(&(C,A),A))))";
            Proposition root = ParseExpression.Parse(ref testInput);
            List<VariableProposition> variables = ParseExpression.GetVariables();
            Assert.AreEqual(2, variables.Count);
        }

        [TestMethod]
        public void TestSimpleQuantifier()
        {
            string testInput = "!x.(P)";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(ExistentialQuantifier));
            Assert.AreEqual(false, ParseExpression.IsProposition);
        }

        [TestMethod]
        public void TestIsPropositionFormula()
        {
            string testInput = "&(P,>(Q,|(R,S)))";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.AreEqual(true, ParseExpression.IsProposition);
        }

        [TestMethod]
        public void TestNotQuantifier()
        {
            string testInput = "~(@x.(P(x)))";
            Proposition root = ParseExpression.Parse(ref testInput);
            Assert.IsInstanceOfType(root, typeof(NegationProposition));
            Assert.IsInstanceOfType(((NegationProposition)root).LeftNode, typeof(UniversalQuantifier));
        }
    }
}
