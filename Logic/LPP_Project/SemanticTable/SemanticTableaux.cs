namespace LPP_Project.SemanticTable
{
    using Propositions;
    using System.Collections.Generic;

    public static class SemanticTableaux
    {
        public static Element CreateSemanticTableaux(Proposition root)
        {
            NegationProposition not = new NegationProposition(root);
            Element element = new Element(new List<Proposition> { not }, new List<char>());
            element.GenerateNewNodes();

            return element;
        }
    }
}
