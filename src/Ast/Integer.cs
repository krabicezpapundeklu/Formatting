namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Integer : Expression
    {
        public int Value { get; private set; }

        public Integer(int value)
            : this(Location.Unknown, value)
        {
        }

        public Integer(Location location, int value)
            : base(location)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Integer(newLocation, Value);
        }
    }
}
