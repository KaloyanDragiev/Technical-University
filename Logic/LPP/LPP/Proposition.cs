using System.Collections.Generic;

namespace LPP
{
    public abstract class Proposition : IProposition
    {
        private static int nextId = 0;

        public Proposition()
        {
            Id = nextId++;
        }

        public int Id { get; set; }
        public abstract Proposition Nandify();
        public abstract string ToGraph();
        public abstract string ToInfixString();
        public abstract List<char> GenerateTruthTable();
        public abstract List<Predicate> GetPredicates();
        public abstract Proposition Copy();
    }
}
