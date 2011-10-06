namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class Format : FormatStringItem
    {
        protected Format(Location location, Expression argument)
            : base(location)
        {
            Argument = Utilities.ThrowIfNull(argument, "argument");
        }

        public Expression Argument { get; private set; }
    }
}
