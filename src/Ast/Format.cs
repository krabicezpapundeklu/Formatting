namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class Format : AstNode, IFormatStringItem
    {
        public ArgumentIndex ArgumentIndex { get; private set; }

        protected Format(Location location, ArgumentIndex argumentIndex)
            : base(location)
        {
            if(argumentIndex == null)
            {
                throw new ArgumentNullException("argumentIndex");
            }

            ArgumentIndex = argumentIndex;
        }

        public Format Clone(Location newLocation)
        {
            if(newLocation == null)
            {
                throw new ArgumentNullException("newLocation");
            }

            return DoClone(newLocation);
        }

        protected abstract Format DoClone(Location newLocation);
    }
}
