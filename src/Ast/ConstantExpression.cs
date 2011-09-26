namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class ConstantExpression : Expression
    {
        public ConstantExpression(object constant, string text)
            : this(Location.Unknown, constant, text) {}

        public ConstantExpression(Location location, object constant)
            : this(location, constant, constant == null
                ? "null"
                : constant.ToString()) {}

        public ConstantExpression(Location location, object constant, string text)
            : base(location)
        {
            if(text == null)
                throw new ArgumentNullException("text");

            Constant = constant;
            Text = text;
        }

        public object Constant { get; private set; }
        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }

        protected override void DoAccept(IAstVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ConstantExpression(newLocation, Constant, Text);
        }
    }
}
