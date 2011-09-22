﻿namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using T = Token;

    public class TokenInfo
    {
        public int Token { get; private set; }
        public string Text { get; private set; }
        public Location Location { get; private set; }

        public TokenInfo(int token, string text, Location location)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            if(location == null)
            {
                throw new ArgumentNullException("location");
            }

            Token = token;
            Text = text;
            Location = location;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<{0}, {1}, {2}", T.ToString(Token), Location.Start, Location.End);

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
