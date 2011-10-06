﻿namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    public static class Utilities
    {
        public static string Escape(string text)
        {
            ThrowIfNull(text, "text");

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

        public static T ThrowIfNull<T>(T value, string name) where T : class
        {
            if(name == null)
                throw new ArgumentNullException("name");

            if(value == null)
                throw new ArgumentNullException(name);

            return value;
        }
    }
}