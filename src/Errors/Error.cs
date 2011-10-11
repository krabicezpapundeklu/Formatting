namespace Krabicezpapundeklu.Formatting.Errors
{
    using System;

    public class Error : IEquatable<Error>, ILocated
    {
        #region Constructors and Destructors

        public Error(Location location, string description)
        {
            this.Location = Utilities.ThrowIfNull(location, "location");
            this.Description = Utilities.ThrowIfNull(description, "description");
        }

        #endregion

        #region Public Properties

        public string Description { get; private set; }

        public Location Location { get; private set; }

        #endregion

        #region Public Methods

        public bool Equals(Error other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Description == this.Description
                   && LocationComparer.Instance.Compare(other.Location, this.Location) == 0;
        }

        #endregion
    }
}