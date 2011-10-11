namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Text : FormatStringItem
    {
        #region Constructors and Destructors

        public Text(Location location, string value)
            : base(location)
        {
            this.Value = Utilities.ThrowIfNull(value, "value");
        }

        #endregion

        #region Public Properties

        public string Value { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Utilities.Escape(this.Value);
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Text(newLocation, this.Value);
        }

        #endregion
    }
}