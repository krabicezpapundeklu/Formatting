namespace Krabicezpapundeklu.Formatting.Errors
{
    public class SimpleErrorLogger : ErrorLogger
    {
        protected override void DoLogError(Location location, string description)
        {
            throw new FormattingException(location, description);
        }
    }
}
