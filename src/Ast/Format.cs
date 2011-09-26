namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class Format : FormatStringItem
    {
        protected Format(ArgumentIndex argumentIndex)
            : this(Location.Unknown, argumentIndex) {}

        protected Format(Location location, ArgumentIndex argumentIndex)
            : base(location)
        {
            if(argumentIndex == null)
                throw new ArgumentNullException("argumentIndex");

            ArgumentIndex = argumentIndex;
        }

        public ArgumentIndex ArgumentIndex { get; private set; }
    }
}
