namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class AstNode : ILocated
    {
        protected AstNode(Location location)
        {
            Location = Utilities.ThrowIfNull(location, "location");
        }

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion

        public object Accept(IAstVisitor visitor)
        {
            return DoAccept(Utilities.ThrowIfNull(visitor, "visitor"));
        }

        public AstNode Clone(Location newLocation)
        {
            return DoClone(Utilities.ThrowIfNull(newLocation, "newLocation"));
        }

        protected abstract object DoAccept(IAstVisitor visitor);
        protected abstract AstNode DoClone(Location newLocation);
    }
}
