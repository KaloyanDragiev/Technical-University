namespace LPP_Project
{
    using System;
    using Propositions;
    using System.Linq;
    using System.Collections.Generic;

    public class TruthTable
    {
        private List<TruthTableRow> table;

        public long Rows { get; set; }
        public int Cols { get; set; }

        public int InnerCounter { get; set; }

        public TruthTable(Proposition tree)
        {
            this.Rows = ParseExpression.TwoPowX(ParseExpression.GetVariables().Count);
            this.Cols = ParseExpression.GetVariables().Count;

            this.PopulateMatrix(this.Rows, this.Cols);
            this.CalculateTable(tree);
        }

        public TruthTable()
        {
            this.table = new List<TruthTableRow>();
        }

        public List<TruthTableRow> GetTable()
        {
            return this.table;
        }

        public void PopulateMatrix(long rows, int cols)
        {
            this.table = new List<TruthTableRow>();

            for (int i = 0; i < rows; i++)
            {
                this.table.Add(new TruthTableRow
                {
                    Value = new string[cols]
                });
            }

            FillCombinationsRow("01", cols, this.table, 0);
        }

        public Proposition DisjunctiveNormalForm(List<TruthTableRow> tTable)
        {
            Proposition functionOr = null;
            int counter = 0;

            string result = "";
            foreach (var truthTableRow in tTable)
            {
                result += truthTableRow.CalculatedValue;
            }

            if (result.Distinct().Count() == 1 && result.Distinct().Contains('1'))
            {
                return new TrueProposition();
            }
            if (result.Distinct().Count() == 1 && result.Distinct().Contains('0'))
            {
                return new FalseProposition();
            }

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '1')
                {
                    string currentRow = string.Join("", tTable[i].Value);//000 1
                    this.InnerCounter = 0;
                    Proposition function = this.GetFunctionOnlyDisjunctive(currentRow);//(A^(B^C))

                    if (counter == 0)
                    {
                        functionOr = function;
                    }
                    else
                    {
                        functionOr = new OrProposition(functionOr, function);//(A^(B^C)) V (A^(B^C)) 
                    }
                    counter++;
                }
            }

            return functionOr ?? new FalseProposition();
        }

        private Proposition GetFunctionOnlyDisjunctive(string row)
        {
            if (row.Length == 1)
            {
                if (row[0] == '0')
                {
                    return new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
                }
                
                return ParseExpression.GetVariables()[this.InnerCounter];
            }

            Proposition function = null;
            if (row[0] == '0')
            {
                function = new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
            }
            else if (row[0] == '1')
            {
                function = ParseExpression.GetVariables()[this.InnerCounter];
            }

            this.InnerCounter++;
            return new AndProposition(function, this.GetFunctionOnlyDisjunctive(row.Substring(1)));
        }

        private Proposition GetFunction(string row)
        {
            string temp = row;

            if (row[0] == '*')
            {//***0*
                for (int i = 0; i < row.Length-1; i+=2)
                {
                    if (row[i] == '*')
                    {
                        //**1
                        this.InnerCounter++;
                        temp = temp.Substring(1);
                        if (row.Length - 1 > i)
                        {
                            if (row[i+1] != '*')
                            {
                                break;
                            }
                            this.InnerCounter++;
                            temp = temp.Substring(1);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                row = temp;
            }
            
            if (row.Length == 1)
            {
                if (row[0] == '0')
                {
                    return new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
                }

                return ParseExpression.GetVariables()[this.InnerCounter];
            }

            Proposition function = null;
            if (row[0] == '0')
            {
                function = new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
            }
            else if (row[0] == '1')
            {
                function = ParseExpression.GetVariables()[this.InnerCounter];
            }

            //1*
            if (row.Substring(1, 1) == "*" && row.Length == 2)//if next row is last row equals to * then return
            {
                if (row[0] == '0')
                {
                    return new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
                }

                return ParseExpression.GetVariables()[this.InnerCounter];
            }
            //1***
            if (row.Substring(1).All(c => "*".Contains(c)))//if next row contains only *
            {
                if (row[0] == '0')
                {
                    return new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
                }

                return ParseExpression.GetVariables()[this.InnerCounter];
            }
            //1*1
            if (row.Substring(1,1) == "*")//if next row is last row equals to * then return
            {
                if (row[0] == '0')
                {
                    function = new NegationProposition(ParseExpression.GetVariables()[this.InnerCounter]);
                }
                else
                {
                    function = ParseExpression.GetVariables()[this.InnerCounter];
                }

            }

            this.InnerCounter++;
            return new AndProposition(function, this.GetFunction(row.Substring(1)));
        }

        public Proposition DisjunctiveNormalFormSimplified(List<TruthTableRow> tTable)
        {
            Proposition functionOr = null;
            int counter = 0;

            string result = "";
            foreach (var truthTableRow in tTable)
            {
                result += truthTableRow.CalculatedValue;
            }

            if (result.Distinct().Count() == 1 && result.Distinct().Contains('1'))
            {
                return new TrueProposition();
            }
            if (result.Distinct().Count() == 1 && result.Distinct().Contains('0'))
            {
                return new FalseProposition();
            }

            foreach (TruthTableRow row in tTable)
            {
                if (row.CalculatedValue == "1")
                {
                    string currentRow = string.Join("", row.Value);//000 1
                    this.InnerCounter = 0;
                    //***
                    Proposition function = currentRow.Distinct().Count() == 1 && currentRow.Distinct().Contains('*') ? new TrueProposition() : this.GetFunction(currentRow);

                    if (counter == 0)
                    {
                        functionOr = function;
                    }
                    else
                    {
                        functionOr = new OrProposition(functionOr, function);//(A^(B^C)) V (A^(B^C)) 
                    }
                    counter++;
                }
            }

            return functionOr ?? new FalseProposition();
        }

        public TruthTable SimplifyTruthTable(TruthTable simplifyTable)
        {
            TruthTable newTable = new TruthTable();
            
            for (int i = 0; i < simplifyTable.table.Count; i++)//Go through the rows
            {
                for (int j = i + 1; j < simplifyTable.table.Count; j++)//Go through the next rows
                {
                    if (simplifyTable.table[i].CalculatedValue == simplifyTable.table[j].CalculatedValue)//check the same result 
                    {
                        bool differsOnce = CheckRowDiffers(simplifyTable.table[i], simplifyTable.table[j]);//check one different sign

                        if (differsOnce)
                        {
                            PutNewRow(simplifyTable.table[i], simplifyTable.table[j], newTable);//put row with *
                            simplifyTable.table[i].Status = true;
                            simplifyTable.table[j].Status = true;
                        }
                    }
                }
                if (!simplifyTable.table[i].Status)
                {
                    PutDifferentRow(simplifyTable.table[i], newTable);//put not matching the row
                }
            }

            return CheckForDuplicates(newTable);//remove duplicates
        }

        private static TruthTable CheckForDuplicates(TruthTable newTable)
        {
            TruthTable cleanTable = newTable; 

            for (int i = 0; i < newTable.table.Count; i++)
            {
                string currentRow = string.Join("", newTable.table[i].Value);
                for (int j = i + 1; j < newTable.table.Count; j++)
                {
                    string nextRow = string.Join("", newTable.table[j].Value);
                    if (currentRow == nextRow)
                    {
                        cleanTable.table.RemoveAt(j);
                    }
                }
            }

            return cleanTable;
        }

        private static void PutDifferentRow(TruthTableRow first, TruthTable newTableRow)
        {
            newTableRow.table.Add(new TruthTableRow
            {
                Value = new string[first.Value.Length]
            });

            for (int i = 0; i < first.Value.Length; i++)
            {
                newTableRow.table[newTableRow.table.Count - 1].Value[i] = first.Value[i];
            }

            newTableRow.table[newTableRow.table.Count - 1].CalculatedValue = first.CalculatedValue;
        }

        private static void PutNewRow(TruthTableRow first, TruthTableRow second, TruthTable newTableRow)
        {
            newTableRow.table.Add(new TruthTableRow
            {
                Value = new string[first.Value.Length]
            });

            for (int i = 0; i < first.Value.Length; i++)
            {
                if (first.Value[i] != second.Value[i])
                {
                    newTableRow.table[newTableRow.table.Count - 1].Value[i] = "*";
                }
                else
                {
                    newTableRow.table[newTableRow.table.Count - 1].Value[i] = first.Value[i];
                }
            }

            newTableRow.table[newTableRow.table.Count - 1].CalculatedValue = first.CalculatedValue;
        }

        private static bool CheckRowDiffers(TruthTableRow first, TruthTableRow second)
        {
            int counter = 0;

            for (int i = 0; i < first.Value.Length; i++)
            {
                if (first.Value[i] != second.Value[i])
                {
                    counter++;
                }
            }

            return counter == 1;
        }

        private static void FillCombinationsRow(string alphabet, int columns, List<TruthTableRow> givenTable, int counter)
        {
            char[] temp = new char[columns];

            void Combination(int col)
            {
                if (col == 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i] == '0')
                        {
                            givenTable[counter].Value[i] = "0";
                        }
                        else
                        {

                            givenTable[counter].Value[i] = "1";
                        }
                    }
                    counter++;
                }
                else
                {
                    foreach (var letter in alphabet)
                    {
                        temp[columns - col] = letter;
                        Combination(col - 1);
                    }
                }
            }

            Combination(columns);
        }

        private void CalculateTable(Proposition tree)
        {
            foreach (TruthTableRow row in this.table)
            {
                string[] tableRowValues = row.Value;
                row.CalculatedValue = tree.CalculateRoot(tableRowValues.Select(x => x != "0").ToArray()).ToString();
            }
        }

        public string GetHashcode(TruthTable tTable)
        {
            string hexadecimalCode = "";

            foreach (TruthTableRow row in tTable.GetTable())
            {
                hexadecimalCode += row.CalculatedValue;
            }

            hexadecimalCode = GetHashCodeToDisplay(hexadecimalCode);
            return hexadecimalCode;
        }

        private static string GetHashCodeToDisplay(string hashCode)
        {
            char[] array = hashCode.ToCharArray();
            Array.Reverse(array);
            hashCode = new string(array);
            if (hashCode.Length > 8)
            {
                string hex = string.Join("",
                    Enumerable.Range(0, hashCode.Length / 8)
                        .Select(i => Convert.ToByte(hashCode.Substring(i * 8, 8), 2).ToString("X2")));
                return hex;
            }

            return Convert.ToInt32(hashCode, 2).ToString("X");
        }

        public TruthTable SimplifyMultipleTimes(TruthTable givenTable)
        {
            TruthTable firstSimplify = givenTable.SimplifyTruthTable(givenTable); 
            TruthTable secondSimplify = givenTable.SimplifyTruthTable(firstSimplify);
            TruthTable thirdSimplify = givenTable.SimplifyTruthTable(secondSimplify);
            TruthTable fourSimplify = givenTable.SimplifyTruthTable(thirdSimplify);
            TruthTable fiveSimplify = givenTable.SimplifyTruthTable(fourSimplify);
            TruthTable sixSimplify = givenTable.SimplifyTruthTable(fiveSimplify);
            TruthTable sevenSimplify = givenTable.SimplifyTruthTable(sixSimplify);
            TruthTable eightSimplify = givenTable.SimplifyTruthTable(sevenSimplify);
            TruthTable nineSimplify = givenTable.SimplifyTruthTable(eightSimplify);

            return nineSimplify;
        }
    }
}
