namespace Calculator.Draw
{
    using System.Diagnostics;
    using System.Windows.Forms;

    public class DrawBinaryTree
    {
        private readonly Tree tree;
        private readonly Node node;
        private readonly Process dot;
        private readonly PictureBox pictureBox;

        public DrawBinaryTree(Tree tree, PictureBox pictureBox)
        {
            this.tree = tree;
            this.pictureBox = pictureBox;
            this.dot = new Process();

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            this.DrawTree(tree.Derivative != null ? tree.Derivative.Simplify().GenerateBinaryTree() : tree.Root.Simplify().GenerateBinaryTree());
        }

        public DrawBinaryTree(Node node, PictureBox pictureBox)
        {
            this.node = node;
            this.pictureBox = pictureBox;
            this.dot = new Process();

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            this.DrawTree(node.GenerateBinaryTree());
        }

        public void DrawTree(string generateTree)
        {
            System.IO.File.WriteAllText("abc.dot",
                "graph calculus {\nnode[fontname = \"Arial\"]\n" + generateTree + "}");

            this.dot.StartInfo.FileName = "dot.exe";
            this.dot.StartInfo.Arguments = "-Tpng -oabc.png abc.dot";

            this.dot.Start();
            this.dot.WaitForExit();

            this.pictureBox.ImageLocation = "abc.png";
        }
        
    }
}
