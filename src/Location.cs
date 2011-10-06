namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Location : IEquatable<Location>
    {
        public static readonly Location Unknown = new Location();

        private Location() {}

        public Location(int start, int end)
        {
            if(start < 0)
                throw new ArgumentOutOfRangeException("start");

            if(end < 0)
                throw new ArgumentOutOfRangeException("end");

            Start = start;
            End = end;
        }

        public int Start { get; private set; }
        public int End { get; private set; }

        public bool IsKnown
        {
            get { return this != Unknown; }
        }

        public int Length
        {
            get { return End - Start; }
        }

        #region IEquatable<Location> Members

        public bool Equals(Location other)
        {
            Utilities.ThrowIfNull(other, "other");

            if(!other.IsKnown)
                return !IsKnown;

            return other.Start == Start && other.End == End;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public static Location FromRange(params ILocated[] items)
        {
            return FromRange((IEnumerable<ILocated>)items);
        }

        public static Location FromRange(IEnumerable<ILocated> items)
        {
            Utilities.ThrowIfNull(items, "items");

            bool hasSomeItem = false;

            int start = int.MaxValue;
            int end = int.MinValue;

            foreach(ILocated item in items.Where(x => x != null && x.Location != Unknown))
            {
                hasSomeItem = true;
                start = Math.Min(start, item.Location.Start);
                end = Math.Max(end, item.Location.End);
            }

            return hasSomeItem
                ? new Location(start, end)
                : Unknown;
        }

        public override int GetHashCode()
        {
            return 1 + Start ^ End;
        }

        public override string ToString()
        {
            return this == Unknown
                ? "Unknown"
                : string.Format("{0}, {1}", Start, End);
        }
    }
}
