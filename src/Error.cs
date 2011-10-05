namespace Krabicezpapundeklu.Formatting
{
    using System;

    public class Error : IEquatable<Error>, ILocated
    {
        public Error(Location location, string description)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            if(description == null)
                throw new ArgumentNullException("description");

            Location = location;
            Description = description;
        }

        public string Description { get; private set; }

        #region IEquatable<Error> Members

        public bool Equals(Error other)
        {
            if(other == null)
                throw new ArgumentNullException("other");

            return other.Description == Description && LocationComparer.Instance.Compare(other.Location, Location) == 0;
        }

        #endregion

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion
    }
}
