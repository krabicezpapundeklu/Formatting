namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class ConstantExpression : Expression
    {
        public ConstantExpression(Location location, object constant)
            : this(location, constant, constant == null
                ? "null"
                : constant.ToString()) {}

        public ConstantExpression(Location location, object constant, string text)
            : base(location)
        {
            if(text == null)
                throw new ArgumentNullException("text");

            Value = constant;
            Text = text;
        }

        public object Value { get; private set; }
        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConstantExpression(newLocation, Value, Text);
        }
    }
}
