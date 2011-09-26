namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class FormatStringItem : AstNode
    {
        protected FormatStringItem() {}

        protected FormatStringItem(Location location)
            : base(location) {}
    }
}
