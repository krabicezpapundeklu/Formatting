namespace Krabicezpapundeklu.Formatting
{
    using System.Collections.Generic;

    public class LocationComparer : IComparer<ILocated>, IComparer<Location>
    {
        public static readonly LocationComparer Instance = new LocationComparer();

        private LocationComparer() {}

        #region IComparer<ILocated> Members

        public int Compare(ILocated x, ILocated y)
        {
            return Compare(Utilities.ThrowIfNull(x, "x").Location, Utilities.ThrowIfNull(y, "y").Location);
        }

        #endregion

        #region IComparer<Location> Members

        public int Compare(Location x, Location y)
        {
            Utilities.ThrowIfNull(x, "x");
            Utilities.ThrowIfNull(y, "y");

            if(x.Start < y.Start)
                return -1;

            if(x.Start > y.Start)
                return 1;

            if(x.End < y.End)
                return -1;

            return x.End > y.End
                ? 1
                : 0;
        }

        #endregion
    }
}
