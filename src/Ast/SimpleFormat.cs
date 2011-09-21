namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Text;

    public class SimpleFormat : Format
    {
        public bool LeftAlign { get; private set; }
        public int Width { get; private set; }
        public FormatString FormatString { get; private set; }

        public SimpleFormat(ArgumentIndex argumentIndex, bool leftAlign, int width, FormatString formatString)
            : base(argumentIndex)
        {
            if(formatString == null)
            {
                throw new ArgumentNullException("formatString");
            }

            LeftAlign = leftAlign;
            Width = width;
            FormatString = formatString;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append('{');
            builder.Append(ArgumentIndex);

            if(Width > 0)
            {
                builder.Append(',');

                if(LeftAlign)
                {
                    builder.Append('-');
                }

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
    }
}
