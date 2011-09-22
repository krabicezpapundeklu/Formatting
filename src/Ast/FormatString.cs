namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Linq;

    public class FormatString : IAstNode
    {
        public List<IFormatStringItem> Items { get; private set; }

        public Location Location
        {
            get
            {
                return Items.Count == 0
                    ? Location.Unknown
                    : new Location(Items.First().Location.Start, Items.Last().Location.End);
            }
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
