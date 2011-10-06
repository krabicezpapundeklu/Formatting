namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Text;

    public class SimpleFormat : Format
    {
        public SimpleFormat(
            Location location, Expression argument, bool leftAlign, int width, FormatString formatString)
            : base(location, argument)
        {
            if(formatString == null)
                throw new ArgumentNullException("formatString");

            LeftAlign = leftAlign;
            Width = width;
            FormatString = formatString;
        }

        public bool LeftAlign { get; private set; }
        public int Width { get; private set; }
        public FormatString FormatString { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Argument);
            builder.Length--;

            if(Width > 0)
            {
                builder.Append(',');

                if(LeftAlign)
                    builder.Append('-');

                builder.Append(Width);
            }

            string formatString = FormatString.ToString();

            if(formatString.Length > 0)
            {
                builder.Append(':');
                builder.Append(formatString);
            }

            builder.Append('}');

            return builder.ToString();
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new SimpleFormat(newLocation, Argument, LeftAlign, Width, FormatString);
        }
    }
}
