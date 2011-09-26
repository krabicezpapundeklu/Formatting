namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    using T = Token;

    public class Operator : AstNode
    {
        public Operator(int token)
            : this(Location.Unknown, token) {}

        public Operator(int token, string text)
            : this(Location.Unknown, token, text) {}

        public Operator(Location location, int token)
            : this(location, token, T.ToString(token)) {}

        public Operator(Location location, int token, string text)
            : base(location)
        {
            if(text == null)
                throw new ArgumentNullException("text");

            Token = token;
            Text = text;
        }

        public int Token { get; private set; }
        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Operator(newLocation, Token, Text);
        }
    }
}
