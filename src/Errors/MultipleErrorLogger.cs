namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;

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

        protected override void DoLogError(Error error)
        {
            if(!errors.Contains(error))
                errors.Add(error);
        }
    }
}
