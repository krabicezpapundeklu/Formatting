namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class BinaryExpression : IExpression
    {
        public Operator Operator { get; private set; }
        public IExpression LeftExpression { get; private set; }
        public IExpression RightExpression { get; private set; }

        public Location Location { get; private set; }

        public BinaryExpression(Operator binaryOperator, IExpression leftExpression, IExpression rightExpression)
        {
            if(binaryOperator == null)
            {
                throw new ArgumentNullException("binaryOperator");
            }

            if(leftExpression == null)
            {
                throw new ArgumentNullException("leftExpression");
            }

            if(rightExpression == null)
            {
                throw new ArgumentNullException("rightExpression");
            }

            Operator = binaryOperator;
            LeftExpression = leftExpression;
            RightExpression = rightExpression;

            Location = Location.FromRange(leftExpression, rightExpression);
        }

        public override string ToString()
        {
            switch (Operator.Token)
            {
                case ',':
                    return string.Concat(LeftExpression, ',', RightExpression);

                case Token.And:
                    return string.Concat(LeftExpression, RightExpression);

                default:
                    return string.Concat(Operator, RightExpression);
            }
        }
    }
}
