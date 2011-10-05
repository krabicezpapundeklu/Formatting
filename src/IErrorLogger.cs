namespace Krabicezpapundeklu.Formatting
{
    using System.Collections.Generic;

    public interface IErrorLogger
    {
        bool HasErrors { get; }
        IEnumerable<Error> GetErrors();
        void LogError(Location location, string descriptionFormat, params object[] formatArguments);
    }
}
