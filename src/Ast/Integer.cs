namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Integer : ConstantExpression
    {
        #region Constructors and Destructors

        public Integer(Location location, int value)
            : base(location, value)
        {
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Integer(newLocation, (int)this.Value);
        }

        #endregion
    }
}