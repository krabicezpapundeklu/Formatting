namespace Krabicezpapundeklu.Formatting.Errors
{
    using System;

    public class Error : IEquatable<Error>, ILocated
    {
        public Error(Location location, string description)
        {
            Location = Utilities.ThrowIfNull(location, "location");
            Description = Utilities.ThrowIfNull(description, "description");
        }

        public string Description { get; private set; }

        #region IEquatable<Error> Members

        public bool Equals(Error other)
        {
            Utilities.ThrowIfNull(other, "other");

            return other.Description == Description && LocationComparer.Instance.Compare(other.Location, Location) == 0;
        }

        #endregion

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion
    }
}
