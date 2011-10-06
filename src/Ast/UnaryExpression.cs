namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class UnaryExpression : Expression
    {
        public UnaryExpression(Location location, Operator unaryOperator, Expression operand)
            : base(location)
        {
            Operator = Utilities.ThrowIfNull(unaryOperator, "unaryOperator");
            Operand = Utilities.ThrowIfNull(operand, "operand");

            if(Operator.Token != '-')
                throw new ArgumentException(string.Format("\"{0}\" is not unary operator.", Operator), "unaryOperator");
        }

        public Operator Operator { get; private set; }
        public Expression Operand { get; private set; }

        public override string ToString()
        {
            return string.Concat(Operator, Operand);
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new UnaryExpression(newLocation, Operator, Operand);
        }
    }
}
