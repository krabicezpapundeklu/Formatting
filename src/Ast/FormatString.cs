namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class FormatString : AstNode
    {
        public static readonly FormatString Empty = new FormatString(
            Location.Unknown, Enumerable.Empty<FormatStringItem>());

        public FormatString(Location location, IEnumerable<FormatStringItem> items)
            : base(location)
        {
            Items =
                new ReadOnlyCollection<FormatStringItem>(
                    new List<FormatStringItem>(Utilities.ThrowIfNull(items, "items")));
        }

        public ReadOnlyCollection<FormatStringItem> Items { get; private set; }

        public override string ToString()
        {
            return string.Concat(Items.Select(x => x.ToString()));
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new FormatString(newLocation, Items);
        }
    }
}
