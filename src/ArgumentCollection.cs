namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ArgumentCollection : List<Argument>
    {
        public ArgumentCollection() {}

        public ArgumentCollection(IEnumerable<object> arguments)
        {
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            AddRange(arguments.Select(x => new Argument(x)));
        }

        public ArgumentCollection(IEnumerable<Argument> arguments)
        {
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            AddRange(arguments);
        }

        public Argument this[string name]
        {
            get { return Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
        }

        public bool Contains(string name)
        {
            if(name == null)
                throw new ArgumentNullException("name");

            return this[name] != null;
        }
    }
}
