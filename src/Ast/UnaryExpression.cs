namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class UnaryExpression : IExpression
    {
        public Operator Operator { get; private set; }
        public IExpression Operand { get; private set; }

        public Location Location
        {
            get
            {
                return new Location(Operator.Location.Start, Operand.Location.End);
            }
        }

        public UnaryExpression(Operator unaryOperator, IExpression operand)
        {
            if(unaryOperator == null)
            {
                throw new ArgumentNullException("unaryOperator");
            }

            if(operand == null)
            {
                throw new ArgumentNullException("operand");
            }

            Operator = unaryOperator;
            Operand = operand;
        }

        public override string ToString()
        {
            return string.Concat(Operator, Operand);
        }
    }
}
