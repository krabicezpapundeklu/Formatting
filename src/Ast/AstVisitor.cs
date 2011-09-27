namespace Krabicezpapundeklu.Formatting.Ast
{
    using System;

    public abstract class AstVisitor : IAstVisitor
    {
        #region IAstVisitor Members

        public object Visit(ArgumentIndex argumentIndex)
        {
            if(argumentIndex == null)
                throw new ArgumentNullException("argumentIndex");

            return DoVisit(argumentIndex);
        }

        public object Visit(BinaryExpression binaryExpression)
        {
            if(binaryExpression == null)
                throw new ArgumentNullException("binaryExpression");

            return DoVisit(binaryExpression);
        }

        public object Visit(Case @case)
        {
            if(@case == null)
                throw new ArgumentNullException("case");

            return DoVisit(@case);
        }

        public object Visit(ConditionalFormat conditionalFormat)
        {
            if(conditionalFormat == null)
                throw new ArgumentNullException("conditionalFormat");

            return DoVisit(conditionalFormat);
        }

        public object Visit(ConstantExpression constantExpression)
        {
            if(constantExpression == null)
                throw new ArgumentNullException("constantExpression");

            return DoVisit(constantExpression);
        }

        public object Visit(FormatString formatString)
        {
            if(formatString == null)
                throw new ArgumentNullException("formatString");

            return DoVisit(formatString);
        }

        public object Visit(Integer integer)
        {
            if(integer == null)
                throw new ArgumentNullException("integer");

            return DoVisit(integer);
        }

        public object Visit(Operator @operator)
        {
            if(@operator == null)
                throw new ArgumentNullException("operator");

            return DoVisit(@operator);
        }

        public object Visit(SimpleFormat simpleFormat)
        {
            if(simpleFormat == null)
                throw new ArgumentNullException("simpleFormat");

            return DoVisit(simpleFormat);
        }

        public object Visit(Text text)
        {
            if(text == null)
                throw new ArgumentNullException("text");

            return DoVisit(text);
        }

        public object Visit(UnaryExpression unaryExpression)
        {
            if(unaryExpression == null)
                throw new ArgumentNullException("unaryExpression");

            return DoVisit(unaryExpression);
        }

        #endregion

        protected abstract object Default(AstNode node);

        protected virtual object DoVisit(ArgumentIndex argumentIndex)
        {
            return Default(argumentIndex);
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
    }
}
