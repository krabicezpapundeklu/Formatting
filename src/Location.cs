namespace Krabicezpapundeklu.Formatting
{
    using System;

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
    }
}
