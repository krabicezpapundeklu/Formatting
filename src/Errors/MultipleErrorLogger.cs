namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class MultipleErrorLogger : ErrorLogger
    {
        #region Constants and Fields

        private readonly ReadOnlyCollection<Error> errors;

        private readonly List<Error> mutableErrors;

        private bool sorted;

        #endregion

        #region Constructors and Destructors

        public MultipleErrorLogger()
        {
            this.errors = new ReadOnlyCollection<Error>(this.mutableErrors = new List<Error>());
        }

        #endregion

        #region Public Properties

        public int ErrorCount
        {
            get
            {
                return this.Errors.Count;
            }
        }

        public ReadOnlyCollection<Error> Errors
        {
            get
            {
                if (!this.sorted)
                {
                    this.mutableErrors.Sort(LocationComparer.Instance);
                    this.sorted = true;
                }

                return this.errors;
            }
        }

        #endregion

        #region Methods

        protected override void DoLogError(Error error)
        {
            if (!this.mutableErrors.Any(x => x.Location.Equals(error.Location)))
            {
                this.mutableErrors.Add(error);
                this.sorted = false;
            }
        }

        #endregion
    }
}