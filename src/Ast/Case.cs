﻿namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class Case : AstNode
    {
        public Expression Condition { get; private set; }
        public FormatString FormatString { get; private set; }

        public Case(Expression condition, FormatString formatString)
            : this(Location.Unknown, condition, formatString)
        {
        }

        public Case(Location location, Expression condition, FormatString formatString)
            : base(location)
        {
            if(condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            if(formatString == null)
            {
                throw new ArgumentNullException("formatString");
            }

            Condition = condition;
            FormatString = formatString;
        }

        public override string ToString()
        {
            return string.Format("{{{0}:{1}}}", Condition, FormatString);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new Case(newLocation, Condition, FormatString);
        }
    }
}
