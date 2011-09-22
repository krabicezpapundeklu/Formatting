namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class Text : AstNode, IFormatStringItem
    {
        public string Value { get; private set; }

        public Text(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException("value");
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
