using System.Collections.Generic;

namespace LPP
{
    public abstract class BinaryOperator : Proposition
    {
        public Proposition Left { get; set; }
        public Proposition Right { get; set; }

        public override string ToGraph()
        {
            if (Left is Predicate left)
            {
                Left = left.Copy();
            }
            if (Right is Predicate right)
            {
                Right = right.Copy();
            }
            return " " + Id;
        }

        public override List<Predicate> GetPredicates()
        {
            List<Predicate> predicates = new List<Predicate>();
            List<Predicate> left = Left.GetPredicates();
            List<Predicate> right = Right.GetPredicates();
            if (left != null)
            {
                predicates.AddRange(Left.GetPredicates());
            }
            if (right != null)
            {
                predicates.AddRange(Right.GetPredicates());
            }
            return predicates;
        }
    }
}
