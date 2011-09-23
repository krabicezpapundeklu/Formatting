namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentIndex : Expression
    {
        public int Index { get; private set; }

        public ArgumentIndex(int index)
            : this(Location.Unknown, index)
        {
        }

        public ArgumentIndex(Location location, int index)
            : base(location)
        {
            Index = index;
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", Index);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ArgumentIndex(newLocation, Index);
        }
    }
}
