namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class FormatString : IAstNode
    {
        public static readonly FormatString Empty = new FormatString(Enumerable.Empty<IFormatStringItem>());

        public ReadOnlyCollection<IFormatStringItem> Items { get; private set; }
        public Location Location { get; private set; }

        public FormatString(IEnumerable<IFormatStringItem> items)
        {
            if(items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = new ReadOnlyCollection<IFormatStringItem>(new List<IFormatStringItem>(items));
            Location = Location.FromRange(Items);
        }

        public override string ToString()
        {
            return string.Concat(Items.Select(x => x.ToString()));
        }
    }
}
