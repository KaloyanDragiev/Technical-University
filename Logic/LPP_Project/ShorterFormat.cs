using LPP_Project.Propositions;

namespace LPP_Project
{
    using System;

    public static class ShorterFormat
    {
        //(((¬(r) V p) ^ (¬(r) V ¬(q))) ^ (q V p))
        public static string ConvertToShortFormat(string input)
        {
            if (input.Length == 0)
            {
                return "[]";
            }

            string output = "[ ";
            foreach (var section in input.Split(new[] { " ^ " }, StringSplitOptions.RemoveEmptyEntries))
            {
                var letters = section.Split(new []{ " v " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string letter in letters)
                {
                    string newLetter = letter.Replace("(", "");
                    newLetter = newLetter.Replace(")", "");

                    if (newLetter.Contains("¬"))
                    {
                        var a = newLetter.ToLower().Remove(0, 1);
                        output += a;
                    }
                    else
                    {
                        output += newLetter.ToUpper();
                    }
                }

                output += ", ";
            }

            return output.Remove(output.Length - 2) + " ]";
        }
        //(¬A ∨ B ∨ ¬C) ∧ E ∧ (¬A ∨ C ∨ D)
        //[ aBc, E, aCD ]
        public static MultiAnd ConvertFromShortFormat(string input)
        {
            input = input.Replace("[", "");
            input = input.Replace("]", "");
            input = input.Replace(" ", "");

            string[] array = input.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            string output = "";
            MultiAnd and = new MultiAnd();
            foreach (string letters in array)
            {
                output += "(";
                MultiOr or = new MultiOr();
                foreach (var letter in letters)
                {
                    if (char.IsUpper(letter))// 
                    {
                        output += letter;// TODO: check if variable it hasn't already been added !!!!
                        //now each variable as its own value
                        VariableProposition vp = ParseExpression.AddVariableFunction(letter.ToString().ToUpper().ToCharArray()[0]);
                        or.Propositions.Add(vp);
                    }
                    else
                    {
                        output += "¬" + letter.ToString().ToUpper(); 
                        VariableProposition vp = ParseExpression.AddVariableFunction(letter.ToString().ToUpper().ToCharArray()[0]);
                        or.Propositions.Add(new NegationProposition(vp));
                        
                    }
                    //[ A,AC,acd,AD,af,AFa,AFf,CD,Fa,Ff ]
                    output += " V ";
                }

                and.AddProposition(or);
                output = output.Remove(output.Length - 3);

                output += ") ^ ";
            }
            output = output.Remove(output.Length - 3);

            return and;
        }
    }
}