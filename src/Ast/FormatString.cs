namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Linq;

    public class FormatString : IAstNode
    {
        public List<IFormatStringItem> Items { get; private set; }

        public int Start
        {
            get { return Items.Count == 0 ? 0 : Items[0].Start; }
        }

        public int End
        {
            get { return Items.Count == 0 ? 0 : Items.Last().End; }
        }

        public FormatString()
        {
            Items = new List<IFormatStringItem>();
        }

        public override string ToString()
        {
            return string.Concat(Items.Select(x => x.ToString()));
        }
    }
}
