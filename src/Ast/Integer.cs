namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Integer : ConstantExpression
    {
        public Integer(int value)
            : this(Location.Unknown, value) {}

        public Integer(Location location, int value)
            : base(location, value) {}

        protected override void DoAccept(IAstVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Integer(newLocation, (int)Constant);
        }
    }
}
