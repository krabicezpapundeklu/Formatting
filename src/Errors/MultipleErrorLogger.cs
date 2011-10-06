namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class MultipleErrorLogger : ErrorLogger
    {
        private readonly ReadOnlyCollection<Error> errors;
        private readonly List<Error> mutableErrors;
        private bool sorted;

        public MultipleErrorLogger()
        {
            errors = new ReadOnlyCollection<Error>(mutableErrors = new List<Error>());
        }

        public ReadOnlyCollection<Error> Errors
        {
            get
            {
                if(!sorted)
                {
                    mutableErrors.Sort(LocationComparer.Instance);
                    sorted = true;
                }

                return errors;
            }
        }

        public override int ErrorCount
        {
            get { return Errors.Count; }
        }

        public void ThrowOnErrors()
        {
            if(ErrorCount > 0)
                throw new FormattingException(Errors);
        }

        protected override void DoLogError(Error error)
        {
            if(!mutableErrors.Any(x => x.Location.Equals(error.Location)))
            {
                mutableErrors.Add(error);
                sorted = false;
            }
        }
    }
}
