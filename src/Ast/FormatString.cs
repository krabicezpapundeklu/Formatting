namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class FormatString : AstNode
    {
        public static readonly FormatString Empty = new FormatString(Enumerable.Empty<IFormatStringItem>());

        public ReadOnlyCollection<IFormatStringItem> Items { get; private set; }
        
        public FormatString(IEnumerable<IFormatStringItem> items)
            : this(Location.FromRange(items), items)
        {
        }

        public FormatString(Location location, IEnumerable<IFormatStringItem> items)
            : base(location)
        {
            if(items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = new ReadOnlyCollection<IFormatStringItem>(new List<IFormatStringItem>(items));
        }

        public override string ToString()
        {
            return string.Concat(Items.Select(x => x.ToString()));
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new FormatString(newLocation, Items);
        }
    }
}
