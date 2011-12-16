namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Case : AstNode
    {
        #region Constructors and Destructors

        public Case(Location location, Expression condition, FormatString formatString)
            : base(location)
        {
            this.Condition = Utilities.ThrowIfNull(condition, "condition");
            this.FormatString = Utilities.ThrowIfNull(formatString, "formatString");
        }

        #endregion

        #region Public Properties

        public Expression Condition { get; private set; }

        public FormatString FormatString { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Utilities.InvariantFormat("{{{0}:{1}}}", this.Condition, this.FormatString);
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Case(newLocation, this.Condition, this.FormatString);
        }

        #endregion
    }
}