namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using T = Token;

    public class TokenInfo
    {
        public int Token { get; protected set; }
        public string Text { get; protected set; }
        public int Start { get; protected set; }
        public int End { get; protected set; }

        public TokenInfo()
        {
            Token = T.Invalid;
        }

        public void AssignFrom(TokenInfo other)
        {
            if(other == null)
            {
                throw new ArgumentNullException("other");
            }

            Token = other.Token;
            Text = other.Text;
            Start = other.Start;
            End = other.End;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<{0}, {1}, {2}", T.ToString(Token), Start, End);

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
