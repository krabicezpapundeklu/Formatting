namespace Krabicezpapundeklu.Formatting
{
    internal class Helpers
    {
        public static bool MustBeEscaped(char c)
        {
            return c == '\\' || c == '{' || c == '}';
        }
    }
}
