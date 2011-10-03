namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class ArgumentName : Expression
    {
        public ArgumentName(string name)
            : this(Location.Unknown, name) {}

        public ArgumentName(Location location, string name)
            : base(location)
        {
            if(name == null)
                throw new ArgumentNullException("name");

            Name = name;
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
