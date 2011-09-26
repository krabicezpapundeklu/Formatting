namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Linq;
    using System.Text;

    using Ast;

    public class Interpreter : IAstVisitor
    {
        private readonly object[] arguments;
        private readonly IFormatProvider formatProvider;

        public Interpreter(IFormatProvider formatProvider, object[] arguments)
        {
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            this.formatProvider = formatProvider;
            this.arguments = arguments;
        }

        #region IAstVisitor Members

        public object Visit(ArgumentIndex argumentIndex)
        {
            if(argumentIndex == null)
                throw new ArgumentNullException("argumentIndex");

            if(argumentIndex.Index >= arguments.Length)
                throw new FormattingException(argumentIndex.Location, "Argument index is out of range.");

            return arguments[argumentIndex.Index];
        }

        public object Visit(BinaryExpression binaryExpression)
        {
            if(binaryExpression == null)
                throw new ArgumentNullException("binaryExpression");

            int leftOperand;
            int rightOperand;

            ExpectIntegerOperands(binaryExpression, out leftOperand, out rightOperand);

            switch(binaryExpression.Operator.Token)
            {
                case '=':
                    return leftOperand == rightOperand;

                case '!':
                    return leftOperand != rightOperand;

                case '>':
                    return leftOperand > rightOperand;

                case '<':
                    return leftOperand < rightOperand;

                case Token.LessOrEqual:
                    return leftOperand <= rightOperand;

                case Token.GreaterOrEqual:
                    return leftOperand >= rightOperand;

                default:
                    throw new FormattingException(
                        binaryExpression.Operator.Location, "Invalid operator \"{0}\".", binaryExpression.Operator.Text);
            }
        }

        public object Visit(Case @case)
        {
            if(@case == null)
                throw new ArgumentNullException("case");

            return null;
        }

        public object Visit(ConditionalFormat conditionalFormat)
        {
            if(conditionalFormat == null)
                throw new ArgumentNullException("conditionalFormat");

            foreach(Case @case in conditionalFormat.Cases.Where(x => (bool)x.Condition.Accept(this)))
                return @case.FormatString.Accept(this);

            return string.Empty;
        }

        public object Visit(ConstantExpression constantExpression)
        {
            if(constantExpression == null)
                throw new ArgumentNullException("constantExpression");

            return constantExpression.Value;
        }

        public object Visit(FormatString formatString)
        {
            if(formatString == null)
                throw new ArgumentNullException("formatString");

            var builder = new StringBuilder();

            foreach(FormatStringItem item in formatString.Items)
                builder.Append(item.Accept(this));

            return builder.ToString();
        }

        public object Visit(Integer integer)
        {
            if(integer == null)
                throw new ArgumentNullException("integer");

            return integer.Value;
        }

        public object Visit(Operator @operator)
        {
            if(@operator == null)
                throw new ArgumentNullException("operator");

            return null;
        }

        public object Visit(SimpleFormat simpleFormat)
        {
            if(simpleFormat == null)
                throw new ArgumentNullException("simpleFormat");

            object argument = simpleFormat.ArgumentIndex.Accept(this);
            var format = (string)simpleFormat.FormatString.Accept(this);

            string formatted = Format(argument, format);

            if(formatted.Length < simpleFormat.Width)
            {
                var padding = new string(' ', simpleFormat.Width - formatted.Length);

                return simpleFormat.LeftAlign
                    ? string.Concat(formatted, padding)
                    : string.Concat(padding, formatted);
            }

            return formatted;
        }

        public object Visit(Text text)
        {
            if(text == null)
                throw new ArgumentNullException("text");

            return text.Value;
        }

        public object Visit(UnaryExpression unaryExpression)
        {
            if(unaryExpression == null)
                throw new ArgumentNullException("unaryExpression");

            switch(unaryExpression.Operator.Token)
            {
                case '-':
                    object operand = unaryExpression.Operand.Accept(this);

                    if(operand is int)
                        return -(int)operand;

                    throw new FormattingException(
                        unaryExpression.Operator.Location,
                        "Operator \"-\" cannot be applied to operand of type \"{1}\".", operand.GetType());

                default:
                    throw new FormattingException(
                        unaryExpression.Operator.Location, "Invalid operator \"{0}\".", unaryExpression.Operator.Text);
            }
        }

        #endregion

        private void ExpectIntegerOperands(BinaryExpression binaryExpression, out int leftOperand, out int rightOperand)
        {
            object left = binaryExpression.LeftExpression.Accept(this);
            object right = binaryExpression.RightExpression.Accept(this);

            if(left is int && right is int)
            {
                leftOperand = (int)left;
                rightOperand = (int)right;
                return;
            }

            throw new FormattingException(
                binaryExpression.Operator.Location,
                "Operator \"{0}\" cannot be applied to operands of type \"{1}\" and \"{2}\".",
                binaryExpression.Operator.Text, left.GetType(), right.GetType());
        }

        private string Format(object argument, string format)
        {
            if(argument == null)
                return string.Empty;

            ICustomFormatter formatter = formatProvider == null
                ? null
                : formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;

            if(formatter != null)
                return formatter.Format(format, argument, formatProvider);

            if(argument is IFormattable)
                return ((IFormattable)argument).ToString(format, formatProvider);

            return argument.ToString();
        }
    }
}
