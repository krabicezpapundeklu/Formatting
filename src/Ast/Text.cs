namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Text;

    public class Text : AstNode, IFormatStringItem
    {
        public Text(string value)
            : this(Location.Unknown, value) {}

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
            var builder = new StringBuilder();

            foreach(char c in Value)
            {
                if(Helpers.MustBeEscaped(c))
                    builder.Append('\\');

                builder.Append(c);
            }

            return builder.ToString();
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Text(newLocation, Value);
        }
    }
}
