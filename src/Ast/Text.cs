namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Text : FormatStringItem
    {
        public Text(Location location, string value)
            : base(location)
        {
            Value = Utilities.ThrowIfNull(value, "value");
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Utilities.Escape(Value);
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Text(newLocation, Value);
        }
    }
}
