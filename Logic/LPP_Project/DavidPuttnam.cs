namespace LPP_Project
{
    using System;
    using System.Linq;
    using Propositions;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class DavidPuttnam
    {
        public static bool UnSat { get; set; }
        public static bool In { get; set; }
        public static int CounterOutput { get; set; }
        public static List<VariableProposition> Variables { get; set; }
        public static Dictionary<VariableProposition, int> VariablesValues { get; set; }

        public static void Solve(MultiAnd input, List<VariableProposition> listVariables)
        {
            UnSat = false;
            CounterOutput = 0;
            Variables = new List<VariableProposition>();
            VariablesValues = new Dictionary<VariableProposition, int>();

            foreach (var variable in listVariables)
            {
                Variables.Add(new VariableProposition(variable.ToString()));
            }
            
            Console.WriteLine("Start: " + ShorterFormat.ConvertToShortFormat(input.ToString()));

            Algorithm(input);

            DisplayVariablesValues();
        }

        private static void DisplayVariablesValues()
        {
            foreach (var keys in VariablesValues.Keys)
            {
                Console.Write(keys.ToString() + ": " + VariablesValues[keys]);
                Console.WriteLine();
            }
        }

        private static void Algorithm(MultiAnd input)
        {
            CounterOutput += 5;
            if (Variables.Count == 0)
            {
                return;//check if proposition is only True and False
            }

            var variable = Variables[0];//pick a variable
            Variables.RemoveAt(0); //remove variable from the list

            input = RemoveUseless(input);
            input = SolveNonJanus(input, variable);

            if (Variables.Count > 0)//if it is not the last variable
            {
                Algorithm(Resolution(input, variable));
                input = SubstituteSolution(input);

                //&(|(A,|(~(B),~(D))),&(|(~(A),|(~(C),~(D))),&(|(C,|(~(B),~(D))),|(C,~(D)))))
                //&(|(C,|(D,~(C))),&(|(C,|(D,E)),&(|(C,|(D,|(E,~(A)))),&(D,&(|(E,F),F)))))
                //&(A,&(|(A,C),&(|(~(A),|(~(C),~(D))),&(|(A,D),&(|(~(A),~(F)),&(|(A,|(F,~(A))), &(|(A,|(F,~(F))),&(|(C,D),&(|(F,~(A)),|(F,~(F)))))))))))
            }
            else
            {
                if (input.Propositions.Count > 0)
                {
                    UnSat = true; //unsatisfiable
                }
            }

            Analyze(input, variable);

            return;
        }

        private static MultiAnd Copy(MultiAnd input)
        {
            MultiAnd newAnd = new MultiAnd();

            foreach (MultiOr or in input.Propositions)
            {
                MultiOr newOr = new MultiOr();
                foreach (Proposition variable in or.Propositions)
                {
                    if (variable.ToString().Contains("¬"))
                    {
                        newOr.Propositions.Add(new NegationProposition(new VariableProposition(variable.LeftNode.ToString())));
                        
                    }
                    else
                    {
                        newOr.Propositions.Add(new VariableProposition(variable.ToString()));
                    }

                }
                newAnd.Propositions.Add(newOr);
            }

            return newAnd;
        }

        private static MultiAnd RemoveUseless(MultiAnd input)
        {
            List<int> list = new List<int>();

            MultiAnd newAnd = Copy(input);
            MultiAnd toRemove = new MultiAnd();

            for (int i = 0; i < newAnd.Propositions.Count; i++)
            {
                newAnd.Propositions[i].RemoveUseless();

                if (newAnd.Propositions[i].Useless)
                {
                    toRemove.Propositions.Add(newAnd.Propositions[i]);
                }
            }

            newAnd.Propositions.RemoveAll(toRemove.Propositions.Contains);

            for (int i = 0; i < CounterOutput; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("remove-useless: " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
            return newAnd;
        }

        private static MultiAnd SolveNonJanus(MultiAnd input, VariableProposition variable)
        {
            //&(|(A,|(~(A),C)),&(A,D)) - remove all - A
            //&(|(A,|(~(A),C)),|(~(A),D)) - remove all - a
            //&(|(A,|(~(A),C)),&(A,|(~(A),D))) - skip A,a

            bool hasPositive = false;
            bool hasNegative = false;
            MultiAnd newAnd = Copy(input);

            foreach (MultiOr mr in newAnd.Propositions)
            {
                foreach (Proposition mrVariable in mr.Propositions)
                {
                    if (mrVariable.ToString() == variable.ToString())
                    {
                        hasPositive = true;
                    }
                    if (mrVariable.ToString() == "¬(" + variable.ToString() + ")")
                    {
                        hasNegative = true;
                    }
                }
            }

            if (hasNegative && hasPositive)
            {
                for (int i = 0; i < CounterOutput; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("solve-non-janus on " + variable.ToString() + ": " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
                VariablesValues.Add(variable, -1);
                return newAnd;
            }
            if (hasNegative)
            {//remove =  aBC 
                List<int> list = new List<int>();

                MultiAnd toRemove = new MultiAnd();

                for (int i = 0; i < newAnd.Propositions.Count; i++)
                {
                    MultiOr mr = newAnd.Propositions[i];
                    foreach (Proposition mrVariable in mr.Propositions)
                    {
                        if (mrVariable.ToString() == "¬(" + variable.ToString() + ")")
                        {
                            toRemove.Propositions.Add(mr);
                        }
                    }
                }

                newAnd.Propositions.RemoveAll(toRemove.Propositions.Contains);

                for (int i = 0; i < CounterOutput; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("solve-non-janus on " + variable.ToString() + ": " + variable.ToString() + "=false " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
                VariablesValues.Add(variable, 0);
                return newAnd;
            }
            if (hasPositive)
            { //remove =  ABC
                List<int> list = new List<int>();

                MultiAnd toRemove = new MultiAnd();

                for (int i = 0; i < newAnd.Propositions.Count; i++)
                {
                    MultiOr mr = newAnd.Propositions[i];
                    foreach (Proposition mrVariable in mr.Propositions)
                    {
                        if (mrVariable.ToString() == variable.ToString())
                        {
                            toRemove.Propositions.Add(mr);
                        }
                    }
                }

                newAnd.Propositions.RemoveAll(toRemove.Propositions.Contains);

                for (int i = 0; i < CounterOutput; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("solve-non-janus on " + variable.ToString() + ": " + variable.ToString() + "=true " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
                VariablesValues.Add(variable, 1);
                return newAnd;
            }

            for (int i = 0; i < CounterOutput; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("solve-non-janus on " + variable.ToString() + ": " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
            VariablesValues.Add(variable, 1);
            return newAnd;
        }

        private static MultiAnd Resolution(MultiAnd input, VariableProposition variable)
        {
            List<int> list = new List<int>();
            bool variableExists = false;
            MultiAnd newAnd = Copy(input);

            foreach (MultiOr mr in newAnd.Propositions)
            {
                foreach (Proposition mrVariable in mr.Propositions)
                {
                    if (mrVariable.ToString() == variable.ToString() ||
                        mrVariable.ToString() == "¬(" + variable.ToString() + ")")
                    {
                        variableExists = true;//check if Variable exists still in the input
                    }
                }
            }

            if (!variableExists)
            {
                for (int i = 0; i < CounterOutput; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("resolution on " + variable.ToString() + ": " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
                return newAnd;
            }

            MultiAnd addAnd = new MultiAnd();
            MultiAnd toRemove = new MultiAnd();

            foreach (MultiOr mr in newAnd.Propositions)//AC
            {
                foreach (Proposition mrVariable in mr.Propositions)
                {
                    if (mrVariable.ToString() == variable.ToString())
                    {
                        foreach (MultiOr mr2 in newAnd.Propositions)//aB
                        {
                            foreach (Proposition mrVariable2 in mr2.Propositions)
                            {
                                if (mrVariable2.ToString() == "¬(" + variable.ToString() + ")")
                                {
                                    //AC + aB => CB   &(|(A,C),|(~(A),B))
                                    //get indexed in array then remove the multi ors
                                    //create new multior CB and then add it to the input

                                    toRemove.Propositions.Add(mr);
                                    toRemove.Propositions.Add(mr2);

                                    //get all variables without A & a  and create a multior
                                    MultiOr or = ReturnAllButOne(mr, mr2, variable);

                                    addAnd.AddProposition(or);
                                }
                            }
                        }
                    }
                }
            }

            newAnd.Propositions.RemoveAll(toRemove.Propositions.Contains);

            newAnd = newAnd.JoinAnd(addAnd);//join the new multiAnd

            for (int i = 0; i < CounterOutput; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("resolution on " + variable.ToString() + ": " + ShorterFormat.ConvertToShortFormat(newAnd.ToString()));
            return newAnd;
        }

        private static MultiOr ReturnAllButOne(MultiOr positive, MultiOr negative, VariableProposition variable)
        {
            MultiOr newOr = new MultiOr();

            foreach (Proposition mrVariable in positive.Propositions)
            {
                if (mrVariable.ToString() != variable.ToString())
                {
                    if (mrVariable.ToString().Contains("¬"))
                    {
                        newOr.Propositions.Add(new NegationProposition(new VariableProposition(mrVariable.LeftNode.ToString())));
                    }
                    else
                    {
                        newOr.Propositions.Add(new VariableProposition(mrVariable.ToString()));
                    }
                }
            }

            foreach (Proposition mrVariable in negative.Propositions)
            {
                if (mrVariable.ToString() != "¬(" + variable.ToString() + ")")
                {
                    if(mrVariable.ToString().Contains("¬"))
                    {
                        newOr.Propositions.Add(new NegationProposition(new VariableProposition(mrVariable.LeftNode.ToString())));
                    }
                    else
                    {
                        newOr.Propositions.Add(new VariableProposition(mrVariable.ToString()));
                    }
                }
            }

            //check for duplicates F v F

            MultiOr toRemove = new MultiOr();

            for (int i = 0; i < newOr.Propositions.Count - 1; i++)
            {
                for (int j = i + 1; j < newOr.Propositions.Count; j++)
                {
                    if (newOr.Propositions[i].ToString() == newOr.Propositions[j].ToString())
                    {
                        toRemove.Propositions.Add(newOr.Propositions[j]);
                    }
                }
            }

            newOr.Propositions.RemoveAll(toRemove.Propositions.Contains);
            
            return newOr;
        }

        private static MultiAnd SubstituteSolution(MultiAnd input)
        {
            foreach (VariableProposition key in VariablesValues.Keys)
            {
                foreach (MultiOr or in input.Propositions)
                {
                    foreach (Proposition vr in or.Propositions)
                    {
                        if (vr.ToString().Contains(key.ToString()))
                        {
                            if (vr.ToString().Contains("¬"))
                            {
                                ((VariableProposition)vr.LeftNode).ValueDP = VariablesValues[key];
                            }
                            else
                            {
                                ((VariableProposition)vr).ValueDP = VariablesValues[key];
                            }
                        }
                    }
                }
            }

            return input;
        }

        private static void Analyze(MultiAnd input, VariableProposition variable)
        {
            CounterOutput -= 5;
            for (int i = 0; i < CounterOutput; i++)
            {
                Console.Write(" ");
            }

            if (UnSat)
            {
                Console.WriteLine("has-janus: UNSAT");
                return;
            }
            
            bool doneNothing = true;
            bool matter = false;

            foreach (MultiOr or in input.Propositions)
            {
                bool negative = false;
                MultiOr temp = new MultiOr();//store all the variables = Abc

                VariableProposition variableToFind = new VariableProposition("");
                NegationProposition negationVariableToFind = new NegationProposition(null);
                foreach (Proposition vr in or.Propositions)//Find variable who's value is still unknown
                {
                    if (vr.ToString().Contains("¬"))
                    {
                        if (((VariableProposition)vr.LeftNode).ValueDP == -1)
                        {
                            negative = true;
                            variableToFind = (VariableProposition)vr.LeftNode;
                            negationVariableToFind.LeftNode = (VariableProposition)vr.LeftNode;
                        }
                    }
                    else
                    {
                        if (((VariableProposition)vr).ValueDP == -1)
                        {
                            negative = true;
                            variableToFind = (VariableProposition) vr;
                        }
                    }

                    temp.Propositions.Add(vr);
                    //take every variable with 1 & 0 check if there is -1 value if not return result
                    //if yes check if is false then return A = A v false; true = A v true
                }

                if (negative)//check if there is an unknown variable
                {
                    doneNothing = false;
                    List<bool> arr = new List<bool>();//only Abd abd 

                    foreach (Proposition t in temp.Propositions)
                    {
                        if (t.ToString().Contains("¬"))
                        {
                            if (((VariableProposition)t.LeftNode).ValueDP != -1)
                            {
                                arr.Add(((VariableProposition)t.LeftNode).ValueDP == 0 ? true : false);
                            }
                        }
                        else
                        {
                            if (((VariableProposition)t).ValueDP != -1)
                            {
                                arr.Add(((VariableProposition)t).ValueDP == 0 ? false : true);
                            }
                        }
                    }

                    //0 1 1 0 0 1
                    if (arr.Count > 0)
                    {
                        bool result = arr[0];
                        for (int i = 1; i < arr.Count; i++)
                        {
                            result = result | arr[i];
                        }

                        //var a = or.Propositions.FirstOrDefault(x => ((VariableProposition)x).ValueDP == -1);
                        var putVariable = VariablesValues.Keys.FirstOrDefault(x => variableToFind.ToString().Contains(x.ToString()));

                        if (result)//A v True A doesn't matter = True
                        {
                            if (VariablesValues[putVariable] == -1)
                            {
                                if (variableToFind.ToString().Contains("¬"))
                                    ((VariableProposition)variableToFind.LeftNode).ValueDP = 1;
                                else
                                    ((VariableProposition)variableToFind).ValueDP = 1;
                                VariablesValues[putVariable] = 1;
                                variable.ValueDP = 1;
                                //Console.WriteLine("substitute-solution: [ " + putVariable.ToString() + " ]");
                                //Console.WriteLine("solve-non-janus: " + putVariable.ToString() + "= true");
                                matter = true;
                            }
                        }
                        else
                        {
                            if (negationVariableToFind.LeftNode != null) // ~A v False => ~A = false 
                            {
                                if (VariablesValues[putVariable] == -1)
                                {
                                    ((VariableProposition)negationVariableToFind.LeftNode).ValueDP = 0;
                                    VariablesValues[putVariable] = 0;
                                    variable.ValueDP = 0;
                                    //Console.WriteLine("substitute-solution: [ " + putVariable.ToString() + " ]");
                                    //Console.WriteLine("solve-non-janus: " + putVariable.ToString() + "= false");
                                }
                                else 
                                {//if it already has a peviously set value 
                                    if (matter)
                                    {
                                        ((VariableProposition)negationVariableToFind.LeftNode).ValueDP = 0;
                                        VariablesValues[putVariable] = 0;
                                        variable.ValueDP = 0;
                                        //Console.WriteLine("substitute-solution: [ " + putVariable.ToString() + " ]");
                                        //Console.WriteLine("solve-non-janus: " + putVariable.ToString() + "= false");
                                    }
                                    else
                                    {//[AF aF ]  - unsolvable 
                                        Console.WriteLine("has-janus: UNSAT");
                                        return;
                                    }
                                }
                            }
                            else
                            {//if it already has a peviously set value 
                                if (VariablesValues[putVariable] == -1)
                                {
                                    ((VariableProposition)variableToFind).ValueDP = 1;// A v False => A = true
                                    VariablesValues[putVariable] = 1;
                                    variable.ValueDP = 1;
                                    //Console.WriteLine("substitute-solution: [ " + putVariable.ToString() + " ]");
                                    //Console.WriteLine("solve-non-janus: " + putVariable.ToString() + "= true");
                                }
                                else
                                {//[aF AF]  - unsolvable 
                                    Console.WriteLine("has-janus: UNSAT");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {//only [A] or [a] ?   
                        //var b = or.Propositions.FirstOrDefault(x => ((VariableProposition)x).ValueDP == -1);
                        var putVariable = VariablesValues.Keys.FirstOrDefault(x => variableToFind.ToString() == x.ToString());
                        
                        if (temp.ToString().Contains("¬"))
                        {
                            ((VariableProposition)variableToFind).ValueDP = 0;
                            VariablesValues[putVariable] = 0;
                            variable.ValueDP = 0;
                        }
                        else
                        {
                            ((VariableProposition)variableToFind).ValueDP = 1;
                            VariablesValues[putVariable] = 1;
                            variable.ValueDP = 1;
                        }


                        //Console.WriteLine("substitute-solution: [ " + putVariable.ToString() + " ]");
                        //Console.WriteLine("solve-non-janus: " + putVariable.ToString() + "= true");
                    }
                }
            }

            if (doneNothing)
            {
                Console.WriteLine("substitute-solution: [ -- ]");
            }
            else if (variable.ValueDP != -2)
            {
                string isFalse = variable.ValueDP == 1 ? "true" : "false";
                Console.WriteLine("substitute-solution: [ " + variable.ToString() + " ]");
                Console.WriteLine("solve-non-janus: " + variable.ToString() + "= " + isFalse);
            }
        }
    }
}
