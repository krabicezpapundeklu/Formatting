namespace Krabicezpapundeklu.Formatting.Errors
{
    using System;

    public abstract class ErrorLogger : IErrorLogger
    {
        #region IErrorLogger Members

        public void LogError(Error error)
        {
            if(error == null)
                throw new ArgumentNullException("error");

            DoLogError(error);
        }

        public void LogError(Location location, string descriptionFormat, params object[] formatArguments)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            if(descriptionFormat == null)
                throw new ArgumentNullException("descriptionFormat");

            DoLogError(location, string.Format(descriptionFormat, formatArguments));
        }

        #endregion

        protected abstract void DoLogError(Error error);

        protected virtual void DoLogError(Location location, string description)
        {
            DoLogError(new Error(location, description));
        }
    }
}
