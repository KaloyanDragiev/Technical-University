namespace Calculator.Functions
{
    using System;
    using System.Globalization;

    public class RationalNumberNode : Node
    {
        private readonly float numberValue;

        public RationalNumberNode(float number)
        {
            this.numberValue = number;
            this.Label = Helper.DoubleToString(number);
        }

        public override double Calculate(double x)
        {
            return this.numberValue;
        }

        public override Node Simplify()
        {
            return this;
        }

        public override string ToString()
        {
            return this.Label;
        }

        public override Node ReturnDerivative()
        {
            return new RationalNumberNode(0);
        }
    }

    public static class Helper
    {
        public static string DoubleToString(double d)
        {
            string R = d.ToString("R", CultureInfo.InvariantCulture).Replace(",", "");
            int i = R.IndexOf('E');
            if (i < 0)
                return R;
            string G17 = d.ToString("G17", CultureInfo.InvariantCulture);
            if (!G17.Contains("E"))
                return G17.Replace(",", "");
            // manual parsing
            string beforeTheE = R.Substring(0, i);
            int E = Convert.ToInt32(R.Substring(i + 1));
            i = beforeTheE.IndexOf('.');
            if (i < 0)
                i = beforeTheE.Length;
            else
                beforeTheE = beforeTheE.Replace(".", "");
            i += E;
            while (i < 1)
            {
                beforeTheE = "0" + beforeTheE;
                i++;
            }
            while (i > beforeTheE.Length)
            {
                beforeTheE += "0";
            }
            if (i == beforeTheE.Length)
                return beforeTheE;
            return String.Format("{0}.{1}", beforeTheE.Substring(0, i), beforeTheE.Substring(i));
        }
    }
}