namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class UnaryExpression : Expression
    {
        public Operator Operator { get; private set; }
        public Expression Operand { get; private set; }

        public UnaryExpression(Operator unaryOperator, Expression operand)
            : this(Location.FromRange(unaryOperator, operand), unaryOperator, operand)
        {
        }

        public UnaryExpression(Location location, Operator unaryOperator, Expression operand)
            : base(location)
        {
            if (unaryOperator == null)
            {
                throw new ArgumentNullException("unaryOperator");
            }

            if (operand == null)
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

        protected override AstNode DoClone(Location newLocation)
        {
            return new UnaryExpression(newLocation, Operator, Operand);
        }
    }
}
