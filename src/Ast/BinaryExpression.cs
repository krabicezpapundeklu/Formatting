namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class BinaryExpression : IExpression
    {
        public Operator Operator { get; private set; }
        public IExpression LeftExpression { get; private set; }
        public IExpression RightExpression { get; private set; }

        public int Start
        {
            get { return LeftExpression.Start; }
        }

        public int End
        {
            get { return RightExpression.End; }
        }

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
        }

        public override string ToString()
        {
            switch (Operator.Token)
            {
                case Token.And:
                    return string.Concat(LeftExpression, RightExpression);

                default:
                    return string.Concat(Operator, RightExpression);
            }
        }
    }
}
