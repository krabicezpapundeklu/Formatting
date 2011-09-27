namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class AstNode : ICloneable, ILocated
    {
        protected AstNode()
            : this(Location.Unknown) {}

        protected AstNode(Location location)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            Location = location;
        }

        #region ICloneable Members

        public object Clone()
        {
            return DoClone(Location);
        }

        #endregion

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion

        public object Accept(IAstVisitor visitor)
        {
            if(visitor == null)
                throw new ArgumentNullException("visitor");

            return DoAccept(visitor);
        }

        public AstNode Clone(Location newLocation)
        {
            if(newLocation == null)
                throw new ArgumentNullException("newLocation");

            return DoClone(newLocation);
        }

        protected abstract object DoAccept(IAstVisitor visitor);
        protected abstract AstNode DoClone(Location newLocation);
    }
}
