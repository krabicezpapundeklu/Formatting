namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class AstVisitor : IAstVisitor
    {
        #region IAstVisitor Members

        public object Visit(ArgumentIndex argumentIndex)
        {
            return DoVisit(Utilities.ThrowIfNull(argumentIndex, "argumentIndex"));
        }

        public object Visit(ArgumentName argumentName)
        {
            return DoVisit(Utilities.ThrowIfNull(argumentName, "argumentName"));
        }

        public object Visit(BinaryExpression binaryExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(binaryExpression, "binaryExpression"));
        }

        public object Visit(Case @case)
        {
            return DoVisit(Utilities.ThrowIfNull(@case, "case"));
        }

        public object Visit(ConditionalFormat conditionalFormat)
        {
            return DoVisit(Utilities.ThrowIfNull(conditionalFormat, "conditionalFormat"));
        }

        public object Visit(ConstantExpression constantExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(constantExpression, "constantExpression"));
        }

        public object Visit(FormatString formatString)
        {
            return DoVisit(Utilities.ThrowIfNull(formatString, "formatString"));
        }

        public object Visit(Integer integer)
        {
            return DoVisit(Utilities.ThrowIfNull(integer, "integer"));
        }

        public object Visit(Operator @operator)
        {
            return DoVisit(Utilities.ThrowIfNull(@operator, "operator"));
        }

        public object Visit(SimpleFormat simpleFormat)
        {
            return DoVisit(Utilities.ThrowIfNull(simpleFormat, "simpleFormat"));
        }

        public object Visit(Text text)
        {
            return DoVisit(Utilities.ThrowIfNull(text, "text"));
        }

        public object Visit(UnaryExpression unaryExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(unaryExpression, "unaryExpression"));
        }

        #endregion

        protected abstract object Default(AstNode node);

        protected virtual object DoVisit(ArgumentIndex argumentIndex)
        {
            return Default(argumentIndex);
        }

        protected virtual object DoVisit(ArgumentName argumentName)
        {
            return Default(argumentName);
        }

        protected virtual object DoVisit(BinaryExpression binaryExpression)
        {
            return Default(binaryExpression);
        }

        protected virtual object DoVisit(Case @case)
        {
            return Default(@case);
        }

        protected virtual object DoVisit(ConditionalFormat conditionalFormat)
        {
            return Default(conditionalFormat);
        }

        protected virtual object DoVisit(ConstantExpression constantExpression)
        {
            return Default(constantExpression);
        }

        protected virtual object DoVisit(FormatString formatString)
        {
            return Default(formatString);
        }

        protected virtual object DoVisit(Integer integer)
        {
            return Default(integer);
        }

        protected virtual object DoVisit(Operator @operator)
        {
            return Default(@operator);
        }

        protected virtual object DoVisit(SimpleFormat simpleFormat)
        {
            return Default(simpleFormat);
        }

        protected virtual object DoVisit(Text text)
        {
            return Default(text);
        }

        protected virtual object DoVisit(UnaryExpression unaryExpression)
        {
            return Default(unaryExpression);
        }

        protected object Visit(AstNode node)
        {
            return Utilities.ThrowIfNull(node, "node").Accept(this);
        }

        protected T Visit<T>(AstNode node)
        {
            return (T)Visit(node);
        }
    }
}
