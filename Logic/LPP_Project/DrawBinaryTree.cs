namespace LPP_Project
{
    using System.IO;
    using Propositions;
    using System.Diagnostics;
    using System.Windows.Forms;

    public static class DrawBinaryTree
    {
        public static void DrawTree(Proposition multiAnd, PictureBox pictureBox)
        {
            Process dot = new Process();
            string generateTree = multiAnd.Root.Copy().GenerateExpressionTree();
            if (multiAnd is MultiAnd)
            {
                generateTree = ((MultiAnd) multiAnd.Root.Copy()).GenerateCnfExpressionTree();
            }

            File.WriteAllText("abcd.dot",
                "graph logic {\nnode[fontname = \"Arial\"]\n" + generateTree + "}");

            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oabcd.png abcd.dot";

            dot.Start();
            dot.WaitForExit();

            pictureBox.ImageLocation = "abcd.png";
        }

        public static void DrawTreeSemanticTable(string node, PictureBox pictureBox)
        {
            Process dot = new Process();

            File.WriteAllText("abc.dot",
                "graph logic {\nnode[fontname = \"Arial\"]\n" + node + "}");

            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oabc.png abc.dot";

            dot.Start();
            dot.WaitForExit();

            pictureBox.ImageLocation = "abc.png";
        }
    }
}