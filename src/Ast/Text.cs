namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Text;

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
            var builder = new StringBuilder();

            foreach(char c in Value)
            {
                if(c == '\\' || c == '{' || c == '}')
                {
                    builder.Append('\\');
                }

                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
