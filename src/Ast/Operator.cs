namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    using T = Token;

    public class Operator : AstNode
    {
        #region Constructors and Destructors

        public Operator(Location location, int token)
            : this(location, token, T.ToString(token))
        {
        }

        public Operator(Location location, int token, string text)
            : base(location)
        {
            this.Token = token;
            this.Text = Utilities.ThrowIfNull(text, "text");

            if (!IsOperator(token))
            {
                throw new ArgumentException(string.Format("\"{0}\" is not valid operator.", this.Text), "token");
            }
        }

        #endregion

        #region Public Properties

        public bool IsBinary
        {
            get
            {
                return IsBinaryOperator(this.Token);
            }
        }

        public bool IsUnary
        {
            get
            {
                return IsUnaryOperator(this.Token);
            }
        }

        public string Text { get; private set; }

        public int Token { get; private set; }

        #endregion

        #region Public Methods

        public static bool IsBinaryOperator(int token)
        {
            switch (token)
            {
                case '=':
                case '!':
                case '>':
                case '<':
                case ',':
                case T.LessOrEqual:
                case T.GreaterOrEqual:
                case T.And:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsOperator(int token)
        {
            return IsBinaryOperator(token) || IsUnaryOperator(token);
        }

        public static bool IsUnaryOperator(int token)
        {
            return token == '-';
        }

        public override string ToString()
        {
            return this.Text;
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Operator(newLocation, this.Token, this.Text);
        }

        #endregion
    }
}