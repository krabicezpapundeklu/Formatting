namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class Expression : AstNode
    {
        protected Expression(Location location)
            : base(location) {}
    }
}
