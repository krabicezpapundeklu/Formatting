namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Text;

    public class SimpleFormat : Format
    {
        public SimpleFormat(ArgumentIndex argumentIndex, bool leftAlign, int width, FormatString formatString)
            : this(Location.Unknown, argumentIndex, leftAlign, width, formatString) {}

        public SimpleFormat(
            Location location, ArgumentIndex argumentIndex, bool leftAlign, int width, FormatString formatString)
            : base(location, argumentIndex)
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

            builder.Append('{');
            builder.Append(ArgumentIndex.Index);

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

        protected override AstNode DoClone(Location newLocation)
        {
            return new SimpleFormat(newLocation, ArgumentIndex, LeftAlign, Width, FormatString);
        }
    }
}
