namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class Text : FormatStringItem
    {
        public Text(Location location, string value)
            : base(location)
        {
            if(value == null)
                throw new ArgumentNullException("value");

            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return EscapeHelpers.Escape(Value);
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
