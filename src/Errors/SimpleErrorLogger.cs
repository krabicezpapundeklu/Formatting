namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Diagnostics.CodeAnalysis;

    public class SimpleErrorLogger : ErrorLogger
    {
        #region Constants and Fields

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
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