namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class UnaryExpression : Expression
    {
        #region Constructors and Destructors

        public UnaryExpression(Location location, Operator unaryOperator, Expression operand)
            : base(location)
        {
            this.Operator = Utilities.ThrowIfNull(unaryOperator, "unaryOperator");
            this.Operand = Utilities.ThrowIfNull(operand, "operand");

            if (this.Operator.Token != '-')
            {
                throw new ArgumentException(
                    Utilities.InvariantFormat("\"{0}\" is not unary operator.", this.Operator), "unaryOperator");
            }
        }

        #endregion

        #region Public Properties

        public Expression Operand { get; private set; }

        public Operator Operator { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Concat(this.Operator, this.Operand);
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new UnaryExpression(newLocation, this.Operator, this.Operand);
        }

        #endregion
    }
}