namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public class ConditionalFormat : Format
    {
        public ConditionalFormat(Location location, Expression argument, IEnumerable<Case> cases)
            : base(location, argument)
        {
            if(cases == null)
                throw new ArgumentNullException("cases");

            Cases = new ReadOnlyCollection<Case>(new List<Case>(cases));
        }

        public ReadOnlyCollection<Case> Cases { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Argument);
            builder.Length--;

            builder.Append(' ');

            foreach(Case @case in Cases)
                builder.Append(@case);

            builder.Append('}');

            return builder.ToString();
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConditionalFormat(newLocation, Argument, Cases);
        }
    }
}
