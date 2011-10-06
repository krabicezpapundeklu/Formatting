namespace Krabicezpapundeklu.Formatting.Errors
{
    public interface IErrorLogger
    {
        void LogError(Error error);
        void LogError(Location location, string description);
    }
}
