namespace Krabicezpapundeklu.Formatting.Errors
{
    public interface IErrorLogger
    {
        #region Public Methods

        void LogError(Error error);

        void LogError(Location location, string description);

        #endregion
    }
}