namespace Krabicezpapundeklu.Formatting.Errors
{
    public interface IErrorLogger
    {
        int ErrorCount { get; }

        void LogError(Error error);
        void LogError(Location location, string description);
    }
}
