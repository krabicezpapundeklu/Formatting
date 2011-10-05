namespace Krabicezpapundeklu.Formatting.Errors
{
    public interface IErrorLogger
    {
        void LogError(Location location, string descriptionFormat, params object[] formatArguments);
    }
}
