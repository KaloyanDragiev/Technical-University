using LPP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LLPTests
{
    [TestClass]
    public class TableauTests
    {
        [TestMethod]
        public void TestNodeCreation()
        {
            string testInput = "&(P,Q)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
            Assert.IsInstanceOfType(rootNode.Left, typeof(Node));
            Assert.IsInstanceOfType(rootNode.Right, typeof(Node));
        }

        [TestMethod]
        public void TestSimpleTautology()
        {
            string testInput = ">(P,P)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
            rootNode.CheckClosed();
            Assert.AreEqual(true, rootNode.Closed);
            Assert.AreEqual(true, rootNode.Left.Closed);
        }

        [TestMethod]
        public void TestNotTautology()
        {
            string testInput = "&(P,P)";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
            rootNode.CheckClosed();
            Assert.AreEqual(false, rootNode.Closed);
            Assert.AreEqual(false, rootNode.Left.Closed);
        }

        [TestMethod]
        public void TestQuantifierTautology()
        {
            string testInput = ">(!x.(@y.(|(P(x,y),P(x,y)))), @q.(!p.(P(p,q))))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
            rootNode.CheckClosed();
            Assert.AreEqual(true, rootNode.Closed);
            Assert.AreEqual(true, rootNode.Left.Closed);
        }

        [TestMethod]
        public void TestQuantifierNotTautology()
        {
            string testInput = ">(!x.(P(x)), @q.(Q(q)))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
            rootNode.CheckClosed();
            Assert.AreEqual(false, rootNode.Closed);
            Assert.AreEqual(false, rootNode.Left.Closed);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Infinite recursion. Not a tautology.")]
        public void TestEndlessQuantifierNotTautology()
        {
            string testInput = "&(!x.(@y.(|(P(x,y),P(x,y)))), @q.(!p.(P(p,q))))";
            InputReader ir = new InputReader(testInput);
            Proposition root = ir.CheckInput();
            Node rootNode = PropositionManager.SemanticTableaux(root);
        }
    }
}
