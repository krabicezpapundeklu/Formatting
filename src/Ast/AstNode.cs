namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class AstNode : ILocated
    {
        public Location Location { get; private set; }

        protected AstNode()
            : this(Location.Unknown)
        {
        }

        protected AstNode(Location location)
        {
            if(location == null)
            {
                throw new ArgumentNullException("location");
            }

            Location = location;
        }

        public AstNode Clone(Location newLocation)
        {
            if (newLocation == null)
            {
                throw new ArgumentNullException("newLocation");
            }

            return DoClone(newLocation);
        }

        protected abstract AstNode DoClone(Location newLocation);
    }
}
