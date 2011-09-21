namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Linq;

    public class FormatString : AstNode
    {
        public List<FormatStringItem> Items { get; private set; }

        public FormatString()
        {
            Items = new List<FormatStringItem>();
        }

        public override string ToString()
        {
            return string.Concat(Items.Select(x => x.ToString()));
        }
    }
}
