namespace Krabicezpapundeklu.Formatting.Errors
{
    public class SimpleErrorLogger : ErrorLogger
    {
        #region Constants and Fields

        public static readonly SimpleErrorLogger Instance = new SimpleErrorLogger();

        #endregion

        #region Constructors and Destructors

        private SimpleErrorLogger()
        {
        }

        #endregion

        #region Methods

        protected override void DoLogError(Error error)
        {
            throw new FormattingException(error);
        }

        #endregion
    }
}