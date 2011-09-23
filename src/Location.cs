namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;

    public class Location
    {
        public static readonly Location Unknown = new Location();

        public int Start { get; private set; }
        public int End { get; private set; }

        private Location()
        {
        }

        public Location(int start, int end)
        {
            if(start < 0)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            if(end < 0)
            {
                throw new ArgumentOutOfRangeException("end");
            }

            Start = start;
            End = end;
        }

        public static Location FromRange(params ILocated[] items)
        {
            return FromRange((IEnumerable<ILocated>) items);
        }

        public static Location FromRange(IEnumerable<ILocated> items)
        {
            if(items == null)
            {
                throw new ArgumentNullException("items");
            }

            bool hasSomeItem = false;

            int start = int.MaxValue;
            int end = int.MinValue;

            foreach(var item in items)
            {
                hasSomeItem = true;
                start = Math.Min(start, item.Location.Start);
                end = Math.Max(end, item.Location.End);
            }

            return hasSomeItem ? new Location(start, end) : Unknown;
        }
    }
}
