﻿namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Errors;

    public class FormattingException : FormatException
    {
        public FormattingException(Location location, string descriptionFormat, params object[] arguments)
            : this(Enumerable.Repeat(new Error(location, string.Format(descriptionFormat, arguments)), 1)) {}

        public FormattingException(IEnumerable<Error> errors)
            : base("Format is invalid.")
        {
            if(errors == null)
                throw new ArgumentNullException("errors");

            Errors = new ReadOnlyCollection<Error>(new List<Error>(errors));
        }

        public ReadOnlyCollection<Error> Errors { get; private set; }
    }
}
