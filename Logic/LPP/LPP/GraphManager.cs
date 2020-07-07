using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LPP
{
    public class GraphManager
    {
        public static void Generate(string text, Image img)
        {
            img.Source = null;
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(img.Name + ".dot", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                if (text.Contains("{"))
                {
                    text = text.Replace(@"\", @"\\");
                }
                sw.Write("graph logic { node[fontname = \"Arial\"]" + text + "}");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
            Process dot = new Process();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-T png -o " + img.Name + ".png " + img.Name + ".dot";
            dot.Start();
            dot.WaitForExit();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + img.Name + ".png", UriKind.Relative);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.EndInit();
            img.Source = bitmap;
        }

        public static void Clear(Image img)
        {
            File.Delete(img.Name + ".dot");
            File.Delete(img.Name + ".png");
        }
    }
}
