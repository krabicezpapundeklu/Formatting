namespace Krabicezpapundeklu.Formatting.Errors
{
    using System;

    public abstract class ErrorLogger : IErrorLogger
    {
        public void LogError(Location location, string descriptionFormat, params object[] formatArguments)
        {
            if(location == null)
                throw new ArgumentNullException("location");

            if(descriptionFormat == null)
                throw new ArgumentNullException("descriptionFormat");

            DoLogError(location, string.Format(descriptionFormat, formatArguments));
        }

        protected abstract void DoLogError(Location location, string description);
    }
}
