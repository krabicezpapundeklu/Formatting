namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Case : AstNode
    {
        public Case(Location location, Expression condition, FormatString formatString)
            : base(location)
        {
            Condition = Utilities.ThrowIfNull(condition, "condition");
            FormatString = Utilities.ThrowIfNull(formatString, "formatString");
        }

        public Expression Condition { get; private set; }
        public FormatString FormatString { get; private set; }

        public override string ToString()
        {
            return string.Format("{{{0}:{1}}}", Condition, FormatString);
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Case(newLocation, Condition, FormatString);
        }
    }
}
