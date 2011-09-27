namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Location
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

        public static Location FromRange(params ILocated[] items)
        {
            return FromRange((IEnumerable<ILocated>)items);
        }

        public static Location FromRange(IEnumerable<ILocated> items)
        {
            if(items == null)
                throw new ArgumentNullException("items");

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

        public override string ToString()
        {
            return this == Unknown
                ? "Unknown"
                : string.Format("{0}, {1}", Start, End);
        }
    }
}
