﻿namespace Krabicezpapundeklu.Formatting
{
    using System;

    public class FormattingException : FormatException
    {
        public FormattingException(string format, params object[] arguments)
            : this(Location.Unknown, format, arguments) {}

        public FormattingException(Location location, string format, params object[] arguments)
            : base(string.Format(format, arguments))
        {
            if(location == null)
                throw new ArgumentNullException("location");

            Location = location;
        }

        public Location Location { get; private set; }
    }
}