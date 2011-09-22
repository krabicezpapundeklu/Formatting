namespace Krabicezpapundeklu.Formatting.Ast
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConditionalFormat : Format
    {
        public List<Case> Cases { get; private set; }

        public ConditionalFormat(ArgumentIndex argumentIndex)
            : base(argumentIndex)
        {
            Cases = new List<Case>();
        }

        public override string ToString()
        {
            return string.Format("{{{0} {1}}}", ArgumentIndex.Index, string.Concat(Cases.Select(x => x.ToString())));
        }
    }
}
