namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Linq;

    public class ErrorLogger : IErrorLogger
    {
        private readonly List<Error> errors = new List<Error>();

        #region IErrorLogger Members

        public IEnumerable<Error> GetErrors()
        {
            errors.Sort(LocationComparer.Instance);
            return errors;
        }

        public void LogError(Location location, string descriptionFormat, params object[] formatArguments)
        {
            string description = string.Format(descriptionFormat, formatArguments);

            if(
                errors.Any(
                    x => x.Description == description && LocationComparer.Instance.Compare(x.Location, location) == 0))
                return;

            errors.Add(new Error(location, description));
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        #endregion
    }
}
