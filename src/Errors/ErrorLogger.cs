namespace Krabicezpapundeklu.Formatting.Errors
{
    public abstract class ErrorLogger : IErrorLogger
    {
        #region IErrorLogger Members

        public void LogError(Error error)
        {
            DoLogError(Utilities.ThrowIfNull(error, "error"));
        }

        public void LogError(Location location, string descriptionFormat, params object[] formatArguments)
        {
            DoLogError(
                Utilities.ThrowIfNull(location, "location"),
                string.Format(Utilities.ThrowIfNull(descriptionFormat, "descriptionFormat"), formatArguments));
        }

        #endregion

        protected abstract void DoLogError(Error error);

        protected virtual void DoLogError(Location location, string description)
        {
            DoLogError(new Error(location, description));
        }
    }
}
