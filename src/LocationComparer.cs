namespace Krabicezpapundeklu.Formatting
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class LocationComparer : IComparer<ILocated>, IComparer<Location>
    {
        #region Constants and Fields

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly LocationComparer Instance = new LocationComparer();

        #endregion

        #region Constructors and Destructors

        private LocationComparer()
        {
        }

        #endregion

        #region Public Methods

        public int Compare(ILocated x, ILocated y)
        {
            return Compare(Utilities.ThrowIfNull(x, "x").Location, Utilities.ThrowIfNull(y, "y").Location);
        }

        public int Compare(Location x, Location y)
        {
            Utilities.ThrowIfNull(x, "x");
            Utilities.ThrowIfNull(y, "y");

            if (x.Start < y.Start)
            {
                return -1;
            }

            if (x.Start > y.Start)
            {
                return 1;
            }

            if (x.End < y.End)
            {
                return -1;
            }

            return x.End > y.End ? 1 : 0;
        }

        #endregion
    }
}