namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ConditionalFormat : Format
    {
        public ConditionalFormat(ArgumentIndex argumentIndex, IEnumerable<Case> cases)
            : this(Location.Unknown, argumentIndex, cases) {}

        public ConditionalFormat(Location location, ArgumentIndex argumentIndex, IEnumerable<Case> cases)
            : base(location, argumentIndex)
        {
            if(cases == null)
                throw new ArgumentNullException("cases");

            Cases = new ReadOnlyCollection<Case>(new List<Case>(cases));
        }

        public ReadOnlyCollection<Case> Cases { get; private set; }

        public override string ToString()
        {
            return string.Format("{{{0} {1}}}", ArgumentIndex.Index, string.Concat(Cases.Select(x => x.ToString())));
        }

        protected override void DoAccept(IAstVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConditionalFormat(newLocation, ArgumentIndex, Cases);
        }
    }
}
