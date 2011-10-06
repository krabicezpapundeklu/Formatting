namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentName : Expression
    {
        public ArgumentName(Location location, string name)
            : base(location)
        {
            Name = Utilities.ThrowIfNull(name, "name");
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("{{{0}}}", Name);
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ArgumentName(newLocation, Name);
        }
    }
}
