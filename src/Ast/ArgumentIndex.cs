namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentIndex : AstNode, IExpression
    {
        public int Index { get; private set; }

        public ArgumentIndex(Location location, int index)
            : base(location)
        {
            Index = index;
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", Index);
        }
    }
}
