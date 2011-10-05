namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Linq;

    public class SimpleErrorLogger : ErrorLogger
    {
        protected override void DoLogError(Error error)
        {
            throw new FormattingException(Enumerable.Repeat(error, 1));
        }
    }
}
