namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Globalization;
    using System.Text;

    public static class Utilities
    {
        #region Public Methods

        public static string Escape(string text)
        {
            ThrowIfNull(text, "text");

            var builder = new StringBuilder();

            foreach (char c in text)
            {
                if (MustBeEscaped(c))
                {
                    builder.Append('\\');
                }

                builder.Append(c);
            }

            return builder.ToString();
        }

        public static string InvariantFormat(string formatString, params object[] arguments)
        {
            return string.Format(CultureInfo.InvariantCulture, formatString, arguments);
        }

        public static bool MustBeEscaped(char character)
        {
            return character == '\\' || character == '{' || character == '}';
        }

        public static T ThrowIfNull<T>(T value, string name) where T : class
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        #endregion
    }
}