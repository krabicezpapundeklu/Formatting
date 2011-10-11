namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentName : Expression
    {
        #region Constructors and Destructors

        public ArgumentName(Location location, string name)
            : base(location)
        {
            this.Name = Utilities.ThrowIfNull(name, "name");
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{{{0}}}", this.Name);
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ArgumentName(newLocation, this.Name);
        }

        #endregion
    }
}