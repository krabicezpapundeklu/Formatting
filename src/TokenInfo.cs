namespace Krabicezpapundeklu.Formatting
{
    using System.Text;

    using T = Token;

    public class TokenInfo : ILocated
    {
        #region Constructors and Destructors

        public TokenInfo(Location location, int token, string text)
        {
            this.Location = Utilities.ThrowIfNull(location, "location");
            this.Token = token;
            this.Text = Utilities.ThrowIfNull(text, "text");
        }

        #endregion

        #region Public Properties

        public Location Location { get; private set; }

        public string Text { get; private set; }

        public int Token { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<{0}, {1}", T.ToString(this.Token), this.Location);

            if (this.Text.Length > 0)
            {
                builder.Append(": ");
                builder.Append(this.Text);
            }

            builder.Append('>');

            return builder.ToString();
        }

        #endregion
    }
}