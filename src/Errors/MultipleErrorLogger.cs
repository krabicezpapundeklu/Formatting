namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleErrorLogger : ErrorLogger
    {
        private readonly List<Error> errors = new List<Error>();

        public int ErrorCount
        {
            get { return errors.Count; }
        }

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

        protected override void DoLogError(Location location, string description)
        {
            if(errors.Any(x => x.Description == description && x.Location.Equals(location)))
                return;

            errors.Add(new Error(location, description));
        }
    }
}
