namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class Location : IEquatable<Location>
    {
        #region Constants and Fields

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Location Unknown = new Location();

        #endregion

        #region Constructors and Destructors

        public Location(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            if (end < 0)
            {
                throw new ArgumentOutOfRangeException("end");
            }

            this.Start = start;
            this.End = end;
        }

        private Location()
        {
        }

        #endregion

        #region Public Properties

        public int End { get; private set; }

        public bool IsKnown
        {
            get
            {
                return this != Unknown;
            }
        }

        public int Length
        {
            get
            {
                return this.End - this.Start;
            }
        }

        public int Start { get; private set; }

        #endregion

        #region Public Methods

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

            foreach (ILocated item in items.Where(x => x != null && x.Location != Unknown))
            {
                hasSomeItem = true;
                start = Math.Min(start, item.Location.Start);
                end = Math.Max(end, item.Location.End);
            }

            return hasSomeItem ? new Location(start, end) : Unknown;
        }

        public bool Equals(Location other)
        {
            if (other == null)
            {
                return false;
            }

            if (!other.IsKnown)
            {
                return !this.IsKnown;
            }

            return other.Start == this.Start && other.End == this.End;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public override int GetHashCode()
        {
            return this.IsKnown ? 1 + this.Start ^ this.End : 0;
        }

        public override string ToString()
        {
            return this == Unknown ? "Unknown" : Utilities.InvariantFormat("{0}, {1}", this.Start, this.End);
        }

        #endregion
    }
}