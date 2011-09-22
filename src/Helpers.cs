namespace Krabicezpapundeklu.Formatting
{
    class Helpers
    {
        public static bool MustBeEscaped(char c)
        {
            return c == '\\' || c == '{' || c == '}';
        }
    }
}
