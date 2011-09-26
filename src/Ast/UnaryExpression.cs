namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class UnaryExpression : Expression
    {
        public UnaryExpression(Operator unaryOperator, Expression operand)
            : this(Location.FromRange(unaryOperator, operand), unaryOperator, operand) {}

        public UnaryExpression(Location location, Operator unaryOperator, Expression operand)
            : base(location)
        {
            if(unaryOperator == null)
                throw new ArgumentNullException("unaryOperator");

            if(operand == null)
                throw new ArgumentNullException("operand");

            if(unaryOperator.Token != '-')
                throw new ArgumentException(
                    string.Format("\"{0}\" is not unary operator.", unaryOperator), "unaryOperator");

            Operator = unaryOperator;
            Operand = operand;
        }

        public Operator Operator { get; private set; }
        public Expression Operand { get; private set; }

        public override string ToString()
        {
            return string.Concat(Operator, Operand);
        }

        protected override void DoAccept(IAstVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new UnaryExpression(newLocation, Operator, Operand);
        }
    }
}
