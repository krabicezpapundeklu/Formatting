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
            : this(
                Enumerable.Repeat(
                    new Error(
                        Utilities.ThrowIfNull(location, "location"), Utilities.ThrowIfNull(description, "description")),
                    1)) {}

        public FormattingException(IEnumerable<Error> errors)
            : base("Format is invalid.")
        {
            Errors = new ReadOnlyCollection<Error>(new List<Error>(Utilities.ThrowIfNull(errors, "errors")));
        }

        public ReadOnlyCollection<Error> Errors { get; private set; }
    }
}
