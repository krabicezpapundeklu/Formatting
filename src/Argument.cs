namespace Krabicezpapundeklu.Formatting
{
    using System;

    public class Argument
    {
        public Argument(object value)
            : this(string.Empty, value) {}

        public Argument(string name, object value)
        {
            if(name == null)
                throw new ArgumentNullException("name");

            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Name = {0}, Value = {1}", Name, Value);
        }
    }
}
