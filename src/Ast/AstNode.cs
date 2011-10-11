namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class AstNode : ILocated
    {
        #region Constructors and Destructors

        protected AstNode(Location location)
        {
            this.Location = Utilities.ThrowIfNull(location, "location");
        }

        #endregion

        #region Public Properties

        public Location Location { get; private set; }

        #endregion

        #region Public Methods

        public object Accept(IAstVisitor visitor)
        {
            return this.DoAccept(Utilities.ThrowIfNull(visitor, "visitor"));
        }

        public AstNode Clone(Location newLocation)
        {
            return this.DoClone(Utilities.ThrowIfNull(newLocation, "newLocation"));
        }

        #endregion

        #region Methods

        protected abstract object DoAccept(IAstVisitor visitor);

        protected abstract AstNode DoClone(Location newLocation);

        #endregion
    }
}