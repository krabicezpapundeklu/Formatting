namespace Krabicezpapundeklu.Formatting.Errors
{
    public abstract class ErrorLogger : IErrorLogger
    {
        #region IErrorLogger Members

        public abstract int ErrorCount { get; }

        public void LogError(Error error)
        {
            DoLogError(Utilities.ThrowIfNull(error, "error"));
        }

        public void LogError(Location location, string description)
        {
            DoLogError(Utilities.ThrowIfNull(location, "location"), Utilities.ThrowIfNull(description, "description"));
        }

        #endregion

        protected abstract void DoLogError(Error error);

        protected virtual void DoLogError(Location location, string description)
        {
            DoLogError(new Error(location, description));
        }
    }
}
