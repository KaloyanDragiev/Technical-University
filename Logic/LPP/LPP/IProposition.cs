using System.Collections.Generic;

namespace LPP
{
    public interface IProposition
    {
        string ToInfixString();
        Proposition Nandify();
        List<char> GenerateTruthTable();
        string ToGraph();
    }
}
