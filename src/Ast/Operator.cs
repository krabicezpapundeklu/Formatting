namespace Krabicezpapundeklu.Formatting.Ast
{
    using T = Token;

    public class Operator : AstNode
    {
        public int Token { get; private set; }

        public Operator(Location location, int token)
            : base(location)
        {
            Token = token;
        }

        public override string ToString()
        {
            return T.ToString(Token);
        }
    }
}
