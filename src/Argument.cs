namespace Krabicezpapundeklu.Formatting
{
    public class Argument
    {
        public Argument(object value)
            : this(string.Empty, value) {}

        public Argument(string name, object value)
        {
            Name = Utilities.ThrowIfNull(name, "name");
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
