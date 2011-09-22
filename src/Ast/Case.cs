namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public class Case : AstNode
    {
        public IExpression Condition { get; private set; }
        public FormatString FormatString { get; private set; }

        public Case(IExpression condition, FormatString formatString)
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
    }
}
