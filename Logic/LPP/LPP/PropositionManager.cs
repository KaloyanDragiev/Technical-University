using System;
using System.Collections.Generic;
using System.Linq;

namespace LPP
{
    public static class PropositionManager
    {
        public static string GetHash(List<char> array)
        {
            return array.Select((x, i) => x == '1' ? 1 << i : 0).Sum().ToString("X1");
        }

        private static void AddConjunction(AndOperator and, Proposition p)
        {
            if (and.Left == null)
            {
                and.Left = p;
            }
            else if (and.Right == null)
            {
                and.Right = p;
            }
            else
            {
                AndOperator andLower = new AndOperator()
                {
                    Left = and.Right,
                    Right = p
                };
                and.Right = andLower;
            }
        }

        public static Proposition BuildNormalForm(List<Predicate> predicates, List<char> result)
        {
            Proposition normalForm;
            List<Proposition> conjunctions = new List<Proposition>();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] == '1' && result[i] != '-')
                {
                    AndOperator and = new AndOperator();
                    foreach (Predicate p in predicates)
                    {
                        if (p.Values[i] == '0')
                        {
                            NotOperator not = new NotOperator(p);
                            AddConjunction(and, not);
                        }
                        else
                        {
                            AddConjunction(and, p);
                        }
                    }
                    conjunctions.Add(and);
                }
            }
            if (conjunctions.Count > 1)
            {
                normalForm = new OrOperator();
                foreach (Proposition p in conjunctions)
                {
                    if (((OrOperator)normalForm).Left == null)
                    {
                        ((OrOperator)normalForm).Left = p;
                    }
                    else if (((OrOperator)normalForm).Right == null)
                    {
                        ((OrOperator)normalForm).Right = p;
                    }
                    else
                    {
                        OrOperator or = new OrOperator
                        {
                            Left = ((OrOperator)normalForm).Right,
                            Right = p
                        };
                        ((OrOperator)normalForm).Right = or;
                    }
                }
            }
            else
            {
                normalForm = conjunctions[0];
            }
            return normalForm;
        }

        public static void PopulatePredicateValues(List<Predicate> predicates)
        {
            predicates.Sort((x, y) => y.CompareTo(x));
            for (int i = 0; i < (int)Math.Pow(2, predicates.Count); i++)
            {
                for (int j = predicates.Count - 1; j >= 0; j--)
                {
                    if ((i / (int)Math.Pow(2, j)) % 2 != 0)
                    {
                        predicates[j].Values.Add('1');
                    }
                    else
                    {
                        predicates[j].Values.Add('0');
                    }
                }
            }
            predicates.Sort();
        }

        private static Dictionary<int, List<List<char>>> GenerateDict(List<Predicate> predicates, List<char> result) 
        {
            Dictionary<int, List<List<char>>> dict = new Dictionary<int, List<List<char>>>();
            for (int i = 0; i < result.Count; i++)
            {
                List<char> row = new List<char>();
                foreach (Predicate p in predicates)
                {
                    row.Add(p.Values[i]);
                }
                int ones = 0;
                foreach (char c in row)
                {
                    if (c == '1')
                    {
                        ones++;
                    }
                }
                row.Add(result[i]);
                if (!dict.ContainsKey(ones))
                {
                    dict.Add(ones, new List<List<char>> { row });
                }
                else
                {
                    dict[ones].Add(row);
                }
            }
            return dict;
        }

        private static Dictionary<int, List<List<char>>> Simplify(Dictionary<int, List<List<char>>> dict)
        {
            Dictionary<int, List<List<char>>> newDict = new Dictionary<int, List<List<char>>>();
            foreach (int i in dict.Keys)
            {
                List<List<char>> rows = new List<List<char>>();
                foreach (List<char> row in dict[i])
                {
                    rows.Add(new List<char>(row));
                }
                newDict.Add(i, rows);
            }
            foreach (int i in dict.Keys)
            {
                if (dict.ContainsKey(i + 1))
                {
                    for (int l1 = 0; l1 < dict[i].Count; l1++)
                    {
                        for (int l2 = 0; l2 < dict[i + 1].Count; l2++)
                        {
                            if (dict[i].ElementAt(l1).Last() == dict[i + 1].ElementAt(l2).Last())
                            {
                                List<int> mismatches = new List<int>();
                                for (int j = 0; j < dict[i].ElementAt(l1).Count - 1; j++)
                                {
                                    if (dict[i].ElementAt(l1)[j] != dict[i + 1].ElementAt(l2)[j])
                                    {
                                        mismatches.Add(j);
                                    }
                                }
                                if (mismatches.Count == 1)
                                {
                                    newDict[i].ElementAt(l1)[mismatches[0]] = '*';
                                    newDict[i + 1].ElementAt(l2)[mismatches[0]] = '*';
                                    List<char> newRow = new List<char>(dict[i].ElementAt(l1))
                                    {
                                        [mismatches[0]] = '*'
                                    };
                                    List<char> newRow2 = new List<char>(dict[i+1].ElementAt(l2))
                                    {
                                        [mismatches[0]] = '*',
                                        [newRow.Count-1] = '-'
                                    };
                                    newDict[i].ElementAt(l1)[newRow.Count - 1] = '-';
                                    newDict[i+1].ElementAt(l2)[newRow.Count - 1] = '-';
                                    bool contains = true;
                                    for (int m = 0; m < newDict[i].ElementAt(l1).Count; m++)
                                    {
                                        if (newDict[i].ElementAt(l1)[m] != newRow[m])
                                        {
                                            contains = false;
                                            break;
                                        }
                                    }
                                    if (!contains)
                                    {
                                        newDict[i].Add(newRow);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return newDict;
        }      

        private static void RemoveDuplicates(Dictionary<int, List<List<char>>> dict)
        {
            foreach (List<List<char>> rows in dict.Values)
            {
                foreach (List<List<char>> rows2 in dict.Values)
                {
                    foreach (List<char> row in rows)
                    {
                        foreach (List<char> row2 in rows2)
                        {
                            if (row != row2)
                            {
                                bool same = true;
                                for (int i = 0; i < row.Count; i++)
                                {
                                    if (row[i] != row2[i])
                                    {
                                        same = false;
                                        break;
                                    }
                                }
                                if (same)
                                {
                                    row2[row2.Count - 1] = '-';
                                }
                            }                          
                        }
                    }
                }
            }
        }

        public static Dictionary<int, List<List<char>>> SimplifyTruthTable(List<Predicate> predicates, List<char> result)
        {
            Dictionary<int, List<List<char>>> onesPerRow = GenerateDict(predicates, result);
            for (int i = 0; i < predicates.Count; i++)
            {
                onesPerRow = Simplify(onesPerRow);
            }
            RemoveDuplicates(onesPerRow);
            return onesPerRow;                 
        }

        public static void SubstituteVariable(List<Predicate> predicates, char var, char substitution)
        {
            foreach (Predicate p in predicates)
            {
                for (int i = 0; i < p.Variables.Count; i++)
                {
                    if (p.Variables[i] == var)
                    {
                        p.Variables[i] = substitution;
                    }
                }
            }
        }

        public static Node SemanticTableaux(Proposition root)
        {
            NotOperator not = new NotOperator(root);
            Node node = new Node(new List<Proposition>() { not }, new List<char>());
            node.GenerateNodes();
            return node;
        }
    }
}
