namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Linq;

    public class SimpleErrorLogger : ErrorLogger
    {
        public static readonly SimpleErrorLogger Instance = new SimpleErrorLogger();

        private SimpleErrorLogger() {}

        protected override void DoLogError(Error error)
        {
            throw new FormattingException(Enumerable.Repeat(error, 1));
        }
    }
}
