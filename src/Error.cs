namespace Krabicezpapundeklu.Formatting
{
    using System;

    public class Error : ILocated
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

        #region ILocated Members

        public Location Location { get; private set; }

        #endregion
    }
}
