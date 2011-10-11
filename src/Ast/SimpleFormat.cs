namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Text;

    public class SimpleFormat : Format
    {
        #region Constructors and Destructors

        public SimpleFormat(
            Location location, Expression argument, bool leftAlign, int width, FormatString formatString)
            : base(location, argument)
        {
            this.LeftAlign = leftAlign;
            this.Width = width;
            this.FormatString = Utilities.ThrowIfNull(formatString, "formatString");
        }

        #endregion

        #region Public Properties

        public FormatString FormatString { get; private set; }

        public bool LeftAlign { get; private set; }

        public int Width { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(this.Argument);
            builder.Length--;

            if (this.Width > 0)
            {
                builder.Append(',');

                if (this.LeftAlign)
                {
                    builder.Append('-');
                }

                builder.Append(this.Width);
            }

            string formatString = this.FormatString.ToString();

            if (formatString.Length > 0)
            {
                builder.Append(':');
                builder.Append(formatString);
            }

            builder.Append('}');

            return builder.ToString();
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new SimpleFormat(newLocation, this.Argument, this.LeftAlign, this.Width, this.FormatString);
        }

        #endregion
    }
}