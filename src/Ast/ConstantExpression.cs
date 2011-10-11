namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ConstantExpression : Expression
    {
        #region Constructors and Destructors

        public ConstantExpression(Location location, object constant)
            : this(location, constant, constant == null ? "null" : constant.ToString())
        {
        }

        public ConstantExpression(Location location, object constant, string text)
            : base(location)
        {
            this.Value = constant;
            this.Text = Utilities.ThrowIfNull(text, "text");
        }

        #endregion

        #region Public Properties

        public string Text { get; private set; }

        public object Value { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Text;
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConstantExpression(newLocation, this.Value, this.Text);
        }

        #endregion
    }
}