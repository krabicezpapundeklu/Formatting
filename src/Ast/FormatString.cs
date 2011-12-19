namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class FormatString : AstNode
    {
        #region Constants and Fields

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly FormatString Empty = new FormatString(
            Location.Unknown, Enumerable.Empty<FormatStringItem>());

        #endregion

        #region Constructors and Destructors

        public FormatString(Location location, IEnumerable<FormatStringItem> items)
            : base(location)
        {
            this.Items =
                new ReadOnlyCollection<FormatStringItem>(
                    new List<FormatStringItem>(Utilities.ThrowIfNull(items, "items")));
        }

        #endregion

        #region Public Properties

        public ReadOnlyCollection<FormatStringItem> Items { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Concat(this.Items.Select(x => x.ToString()));
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new FormatString(newLocation, this.Items);
        }

        #endregion
    }
}