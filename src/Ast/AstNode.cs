namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class AstNode : ILocated
    {
        protected AstNode()
            : this(Location.Unknown) {}

        protected AstNode(Location location)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            Location = location;
        }

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion

        public void Accept(IAstVisitor visitor)
        {
            if(visitor == null)
                throw new ArgumentNullException("visitor");

            DoAccept(visitor);
        }

        public AstNode Clone(Location newLocation)
        {
            if(newLocation == null)
                throw new ArgumentNullException("newLocation");

            return DoClone(newLocation);
        }

        protected abstract void DoAccept(IAstVisitor visitor);
        protected abstract AstNode DoClone(Location newLocation);
    }
}
