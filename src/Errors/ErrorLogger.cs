namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Linq;

    public class ErrorLogger : IErrorLogger
    {
        private readonly List<Error> errors = new List<Error>();

        public int ErrorCount
        {
            get { return errors.Count; }
        }

        #region IErrorLogger Members

        public void LogError(Location location, string descriptionFormat, params object[] formatArguments)
        {
            string description = string.Format(descriptionFormat, formatArguments);

            if(
                errors.Any(
                    x => x.Description == description && LocationComparer.Instance.Compare(x.Location, location) == 0))
                return;

            errors.Add(new Error(location, description));
        }

        #endregion

        public IEnumerable<Error> GetErrors()
        {
            errors.Sort(LocationComparer.Instance);
            return errors;
        }

        public void ThrowOnErrors()
        {
            if(ErrorCount > 0)
                throw new FormattingException(GetErrors());
        }
    }
}
