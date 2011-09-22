namespace Krabicezpapundeklu.Formatting
{
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
            Token = other.Token;
            Text = other.Text;
            Start = other.Start;
            End = other.End;
        }
    }
}
