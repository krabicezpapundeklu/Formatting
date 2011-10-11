namespace Krabicezpapundeklu.Formatting.Errors
{
    public abstract class ErrorLogger : IErrorLogger
    {
        #region Public Methods

        public void LogError(Error error)
        {
            this.DoLogError(Utilities.ThrowIfNull(error, "error"));
        }

        public void LogError(Location location, string description)
        {
            this.DoLogError(
                Utilities.ThrowIfNull(location, "location"), Utilities.ThrowIfNull(description, "description"));
        }

        #endregion

        #region Methods

        protected abstract void DoLogError(Error error);

        protected virtual void DoLogError(Location location, string description)
        {
            this.DoLogError(new Error(location, description));
        }

        #endregion
    }
}