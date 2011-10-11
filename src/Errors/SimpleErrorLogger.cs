namespace Krabicezpapundeklu.Formatting.Errors
{
    public class SimpleErrorLogger : ErrorLogger
    {
        public static readonly SimpleErrorLogger Instance = new SimpleErrorLogger();

        private SimpleErrorLogger() {}

        protected override void DoLogError(Error error)
        {
            throw new FormattingException(error);
        }
    }
}
