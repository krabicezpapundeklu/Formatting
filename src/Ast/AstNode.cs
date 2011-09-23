namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class AstNode : IAstNode
    {
        public Location Location { get; private set; }

        protected AstNode(Location location)
        {
            if(location == null)
            {
                throw new ArgumentNullException("location");
            }

            Location = location;
        }
    }
}
