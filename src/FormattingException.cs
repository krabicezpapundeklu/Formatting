namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Errors;

    public class FormattingException : FormatException
    {
        public FormattingException(Location location, string description)
            : this(new Error(Utilities.ThrowIfNull(location, "location"), Utilities.ThrowIfNull(description, "description"))) {}

        public FormattingException(Error error)
            : this(Enumerable.Repeat(Utilities.ThrowIfNull(error, "error"), 1))
        {
        }

        public FormattingException(IEnumerable<Error> errors)
            : base("Format is invalid.")
        {
            Errors = new ReadOnlyCollection<Error>(new List<Error>(Utilities.ThrowIfNull(errors, "errors")));
        }

        public ReadOnlyCollection<Error> Errors { get; private set; }
    }
}
