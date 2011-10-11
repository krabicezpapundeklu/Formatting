namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class Expression : AstNode
    {
        #region Constructors and Destructors

        protected Expression(Location location)
            : base(location)
        {
        }

        #endregion
    }
}