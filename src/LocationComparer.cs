namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;

    public class LocationComparer : IComparer<ILocated>, IComparer<Location>
    {
        public static readonly LocationComparer Instance = new LocationComparer();

        #region IComparer<ILocated> Members

        public int Compare(ILocated x, ILocated y)
        {
            if(x == null)
                throw new ArgumentNullException("x");

            if(y == null)
                throw new ArgumentNullException("y");

            return Compare(x.Location, y.Location);
        }

        #endregion

        #region IComparer<Location> Members

        public int Compare(Location x, Location y)
        {
            if(x == null)
                throw new ArgumentNullException("x");

            if(y == null)
                throw new ArgumentNullException("y");

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
