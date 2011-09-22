namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class Format : AstNode, IFormatStringItem
    {
        public ArgumentIndex ArgumentIndex { get; private set; }

        protected Format(ArgumentIndex argumentIndex)
        {
            if(argumentIndex == null)
            {
                throw new ArgumentNullException("argumentIndex");
            }

            ArgumentIndex = argumentIndex;
        }
    }
}
