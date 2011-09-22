namespace Krabicezpapundeklu.Formatting.Ast
{
    using T = Token;

    public class Operator : AstNode
    {
        public int Token { get; private set; }

        public Operator(int token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return T.ToString(Token);
        }
    }
}
