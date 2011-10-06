﻿namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Integer : ConstantExpression
    {
        public Integer(Location location, int value)
            : base(location, value) {}

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Integer(newLocation, (int)Value);
        }
    }
}
