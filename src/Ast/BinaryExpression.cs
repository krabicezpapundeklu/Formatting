namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class BinaryExpression : Expression
    {
        public BinaryExpression(
            Location location, Operator binaryOperator, Expression leftExpression, Expression rightExpression)
            : base(location)
        {
            Operator = Utilities.ThrowIfNull(binaryOperator, "binaryOperator");
            LeftExpression = Utilities.ThrowIfNull(leftExpression, "leftExpression");
            RightExpression = Utilities.ThrowIfNull(rightExpression, "rightExpression");

            if(!Operator.IsBinaryOperator(Operator.Token))
                throw new ArgumentException(
                    string.Format("\"{0}\" is not binary operator.", Operator), "binaryOperator");
        }

        public Operator Operator { get; private set; }
        public Expression LeftExpression { get; private set; }
        public Expression RightExpression { get; private set; }

        public override string ToString()
        {
            switch(Operator.Token)
            {
                case ',':
                    return string.Concat(LeftExpression, ',', RightExpression);

                case Token.And:
                    return string.Concat(LeftExpression, RightExpression);

                default:
                    return string.Concat(Operator, RightExpression);
            }
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new BinaryExpression(newLocation, Operator, LeftExpression, RightExpression);
        }
    }
}
