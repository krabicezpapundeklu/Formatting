namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class Format : FormatStringItem
    {
        #region Constructors and Destructors

        protected Format(Location location, Expression argument)
            : base(location)
        {
            this.Argument = Utilities.ThrowIfNull(argument, "argument");
        }

        #endregion

        #region Public Properties

        public Expression Argument { get; private set; }

        #endregion
    }
}