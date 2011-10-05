namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using T = Token;

    public class TokenInfo : ILocated
    {
        public TokenInfo(Location location, int token, string text)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            if(text == null)
                throw new ArgumentNullException("text");

            Location = location;
            Token = token;
            Text = text;
        }

        public int Token { get; private set; }
        public string Text { get; private set; }

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<{0}, {1}", T.ToString(Token), Location);

            if(Text.Length > 0)
            {
                builder.Append(": ");
                builder.Append(Text);
            }

            builder.Append('>');

            return builder.ToString();
        }
    }
}
