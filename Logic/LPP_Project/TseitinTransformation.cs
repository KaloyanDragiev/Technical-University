namespace LPP_Project
{
    using System;
    using System.Linq;
    using Propositions;
    using System.Collections.Generic;

    public static class TseitinTransformation
    {
        private static readonly List<char> ListOfLetters = "PQRSTUVWXYZ".ToCharArray().ToList();
        private static Proposition returnProposition;
        private static int counter = 0;
        public static void Transform(Proposition multiAnd)
        {
            //Traversal(multiAnd);
            //Console.WriteLine();
        }
        private static void Traversal(Proposition multiAnd)
        {
            if (multiAnd.LeftNode == null && multiAnd.RightNode == null)
                return;

            Traversal(multiAnd.LeftNode);

            if (multiAnd.RightNode != null)
            {
                Traversal(multiAnd.RightNode);
            }

            //Is it only concerning <=> ?
            //How to add the on the same proposition and then delete from it or new one to add or a multiAnd
            //take variable and add to the new proposition to be returned
            //~A
           VariableProposition variable = new VariableProposition(ListOfLetters[counter].ToString());
           returnProposition = new MultiAnd();
           returnProposition = new BiImplicationProposition(multiAnd, variable);
           returnProposition.Cnf();
           counter++;
            
            Console.WriteLine(multiAnd.ToString());
        }
    }
}
