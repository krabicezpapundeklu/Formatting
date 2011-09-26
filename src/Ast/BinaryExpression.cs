namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class BinaryExpression : Expression
    {
        public BinaryExpression(Operator binaryOperator, Expression leftExpression, Expression rightExpression)
            : this(Location.FromRange(leftExpression, rightExpression), binaryOperator, leftExpression, rightExpression) {}

        public BinaryExpression(
            Location location, Operator binaryOperator, Expression leftExpression, Expression rightExpression)
            : base(location)
        {
            if(binaryOperator == null)
                throw new ArgumentNullException("binaryOperator");

            if(leftExpression == null)
                throw new ArgumentNullException("leftExpression");

            if(rightExpression == null)
                throw new ArgumentNullException("rightExpression");

            switch(binaryOperator.Token)
            {
                case '=':
                case '!':
                case '>':
                case '<':
                    // nothing to do
                    break;

                default:
                    throw new ArgumentException(
                        string.Format("\"{0}\" is not binary operator.", binaryOperator), "binaryOperator");
            }

            Operator = binaryOperator;
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
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

        protected override AstNode DoClone(Location newLocation)
        {
            return new BinaryExpression(newLocation, Operator, LeftExpression, RightExpression);
        }
    }
}
