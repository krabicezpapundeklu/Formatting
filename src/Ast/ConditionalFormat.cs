﻿namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ConditionalFormat : Format
    {
        public ReadOnlyCollection<Case> Cases { get; private set; }

        public ConditionalFormat(Location location, ArgumentIndex argumentIndex, IEnumerable<Case> cases)
            : base(location, argumentIndex)
        {
            if(cases == null)
            {
                throw new ArgumentNullException("cases");
            }

            Cases = new ReadOnlyCollection<Case>(new List<Case>(cases));
        }

        public override string ToString()
        {
            return string.Format("{{{0} {1}}}", ArgumentIndex.Index, string.Concat(Cases.Select(x => x.ToString())));
        }

        protected override Format DoClone(Location newLocation)
        {
            return new ConditionalFormat(newLocation, ArgumentIndex, Cases);
        }
    }
}
