namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class BinaryExpression : Expression
    {
        #region Constructors and Destructors

        public BinaryExpression(
            Location location, Operator binaryOperator, Expression leftExpression, Expression rightExpression)
            : base(location)
        {
            this.Operator = Utilities.ThrowIfNull(binaryOperator, "binaryOperator");
            this.LeftExpression = Utilities.ThrowIfNull(leftExpression, "leftExpression");
            this.RightExpression = Utilities.ThrowIfNull(rightExpression, "rightExpression");

            if (!Operator.IsBinaryOperator(this.Operator.Token))
            {
                throw new ArgumentException(
                    Utilities.InvariantFormat("\"{0}\" is not binary operator.", this.Operator), "binaryOperator");
            }
        }

        #endregion

        #region Public Properties

        public Expression LeftExpression { get; private set; }

        public Operator Operator { get; private set; }

        public Expression RightExpression { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            switch (this.Operator.Token)
            {
                case ',':
                    return string.Concat(this.LeftExpression, ',', this.RightExpression);

                case Token.And:
                    return string.Concat(this.LeftExpression, this.RightExpression);

                default:
                    return string.Concat(this.Operator, this.RightExpression);
            }
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new BinaryExpression(newLocation, this.Operator, this.LeftExpression, this.RightExpression);
        }

        #endregion
    }
}