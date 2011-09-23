namespace Krabicezpapundeklu.Formatting.Ast
{
    using T = Token;

    public class Operator : AstNode
    {
        public int Token { get; private set; }

        public Operator(int token)
            : this(Location.Unknown, token)
        {
        }

        public Operator(Location location, int token)
            : base(location)
        {
            Token = token;
        }

        public override string ToString()
        {
            return T.ToString(Token);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Operator(newLocation, Token);
        }
    }
}
