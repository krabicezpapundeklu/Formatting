namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    public class ConditionalFormat : Format
    {
        #region Constructors and Destructors

        public ConditionalFormat(Location location, Expression argument, IEnumerable<Case> cases)
            : base(location, argument)
        {
            this.Cases = new ReadOnlyCollection<Case>(new List<Case>(Utilities.ThrowIfNull(cases, "cases")));
        }

        #endregion

        #region Public Properties

        public ReadOnlyCollection<Case> Cases { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(this.Argument);
            builder.Length--;

            builder.Append(' ');

            foreach (Case @case in this.Cases)
            {
                builder.Append(@case);
            }

            builder.Append('}');

            return builder.ToString();
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConditionalFormat(newLocation, this.Argument, this.Cases);
        }

        #endregion
    }
}