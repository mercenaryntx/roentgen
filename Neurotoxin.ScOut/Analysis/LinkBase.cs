namespace Neurotoxin.ScOut.Analysis
{
    public abstract class LinkBase
    {
        private object Left { get; }
        private object Right { get; }

        protected LinkBase(object left, object right)
        {
            Left = left;
            Right = right;
        }

        private bool Equals(LinkBase other)
        {
            return other.GetType() == GetType() && Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) || Equals((LinkBase) obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = GetType().GetHashCode();
                hashCode = (hashCode * 397) ^ (Left != null ? Left.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}