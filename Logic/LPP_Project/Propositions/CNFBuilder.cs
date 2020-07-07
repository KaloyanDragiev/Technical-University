namespace LPP_Project.Propositions
{
    using System;

    public static class CnfBuilder
    {
        public static MultiAnd MultiAnd = new MultiAnd();

        public static void GenerateMultiAnd(string input)
        {
            MultiAnd = new MultiAnd();
            //"(((¬(r) V p) ^ (¬(r) V ¬(q))) ^ (q V p))"
            String[] strArray = input.Split(new[] { " ^ " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string element in strArray)
            {
                //(((¬(r) V p)
                String[] ors = element.Split(new[] { " V " }, StringSplitOptions.RemoveEmptyEntries);
                MultiOr newMultiOr = new MultiOr();
                foreach (string or in ors)
                {
                    string a = or.Replace(")", "");
                    a = a.Replace("(", "");

                    if (a.Contains("¬"))
                    {
                        if (a.Contains("True"))
                        {
                            a = "~(" + 1 + ")";
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));//= (0,| (A, 0))
                        }
                        else if (a.Contains("False"))
                        {
                            a = "~(" + 0 + ")";
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));
                        }
                        else
                        {
                            a = a.Replace("¬", "");
                            a = "~(" + a + ")";
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));
                        }
                    }
                    else
                    {
                        if (a.Contains("True"))
                        {
                            a = "1";
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));
                        }
                        else if (a.Contains("False"))
                        {
                            a = "0";
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));
                        }
                        else
                        {
                            newMultiOr.Propositions.Add(ParseExpression.Parse(ref a));
                        }
                    }
                }
                MultiAnd.AddProposition(newMultiOr);
            }
        }

        public static Proposition Simplify(Proposition proposition)
        {

            return proposition;
        }
    }
}