namespace Krabicezpapundeklu.Formatting
{
    using System.Text;

    public static class EscapeHelpers
    {
        public static string Escape(string text)
        {
            var builder = new StringBuilder();

            foreach(char c in text)
            {
                if(MustBeEscaped(c))
                    builder.Append('\\');

                builder.Append(c);
            }

            return builder.ToString();
        }

        public static bool MustBeEscaped(char c)
        {
            return c == '\\' || c == '{' || c == '}';
        }
    }
}
