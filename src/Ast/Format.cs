namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class Format : FormatStringItem
    {
        protected Format(Location location, Expression argument)
            : base(location)
        {
            if(argument == null)
                throw new ArgumentNullException("argument");

            Argument = argument;
        }

        public Expression Argument { get; private set; }
    }
}
