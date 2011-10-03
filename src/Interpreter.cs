namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Linq;
    using System.Text;

    using Ast;

    public class Interpreter : AstVisitor
    {
        private readonly object[] arguments;
        private readonly IFormatProvider formatProvider;
        private readonly StringBuilder formatted = new StringBuilder();

        public Interpreter(IFormatProvider formatProvider, object[] arguments)
        {
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            this.formatProvider = formatProvider;
            this.arguments = arguments;
        }

        public string Evaluate(AstNode ast)
        {
            return Visit<string>(ast);
        }

        protected override object Default(AstNode node)
        {
            return null;
        }

        protected override object DoVisit(ArgumentIndex argumentIndex)
        {
            if(argumentIndex.Index >= arguments.Length)
                throw new FormattingException(argumentIndex.Location, "Argument index is out of range.");

            return arguments[argumentIndex.Index];
        }

        protected override object DoVisit(BinaryExpression binaryExpression)
        {
            int leftIntegerOperand;
            int rightIntegerOperand;

            bool leftBooleanOperand;
            bool rightBooleanOperand;

            // TODO: support more operand types... if need ;-)
            switch(binaryExpression.Operator.Token)
            {
                case '=':
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand == rightIntegerOperand;

                case '!':
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand != rightIntegerOperand;

                case '>':
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand > rightIntegerOperand;

                case '<':
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand < rightIntegerOperand;

                case ',':
                    CastOperands(binaryExpression, out leftBooleanOperand, out rightBooleanOperand);
                    return leftBooleanOperand || rightBooleanOperand;

                case Token.LessOrEqual:
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand <= rightIntegerOperand;

                case Token.GreaterOrEqual:
                    CastOperands(binaryExpression, out leftIntegerOperand, out rightIntegerOperand);
                    return leftIntegerOperand >= rightIntegerOperand;

                case Token.And:
                    CastOperands(binaryExpression, out leftBooleanOperand, out rightBooleanOperand);
                    return leftBooleanOperand && rightBooleanOperand;

                default:
                    throw new FormattingException(
                        binaryExpression.Operator.Location, "Invalid operator \"{0}\".", binaryExpression.Operator.Text);
            }
        }

        protected override object DoVisit(ConditionalFormat conditionalFormat)
        {
            // to throw exception if argument is out of range
            Visit(conditionalFormat.ArgumentIndex);

            foreach(Case @case in conditionalFormat.Cases.Where(x => Visit<bool>(x.Condition)))
                return Visit(@case.FormatString);

            return string.Empty;
        }

        protected override object DoVisit(ConstantExpression constantExpression)
        {
            return constantExpression.Value;
        }

        protected override object DoVisit(FormatString formatString)
        {
            int originalLength = formatted.Length;

            foreach(FormatStringItem item in formatString.Items)
                formatted.Append(Visit(item));

            string formattedString = formatted.ToString(originalLength, formatted.Length - originalLength);

            formatted.Length = originalLength;

            return formattedString;
        }

        protected override object DoVisit(Integer integer)
        {
            return integer.Value;
        }

        protected override object DoVisit(SimpleFormat simpleFormat)
        {
            object argument = Visit(simpleFormat.ArgumentIndex);
            var format = Visit<string>(simpleFormat.FormatString);

            string formattedArgument = Format(argument, format);

            int padding = simpleFormat.Width - formattedArgument.Length;

            if(padding > 0)
                if(simpleFormat.LeftAlign)
                {
                    formatted.Append(formattedArgument);
                    formatted.Append(' ', padding);
                }
                else
                {
                    formatted.Append(' ', padding);
                    formatted.Append(formattedArgument);
                }
            else
                formatted.Append(formattedArgument);

            return null;
        }

        protected override object DoVisit(Text text)
        {
            return text.Value;
        }

        protected override object DoVisit(UnaryExpression unaryExpression)
        {
            switch(unaryExpression.Operator.Token)
            {
                case '-':
                    object operand = Visit(unaryExpression.Operand);

                    if(operand is int)
                        return -(int)operand;

                    throw new FormattingException(
                        unaryExpression.Operator.Location,
                        "Operator \"-\" cannot be applied to operand of type \"{0}\".", operand.GetType());

                default:
                    throw new FormattingException(
                        unaryExpression.Operator.Location, "Invalid operator \"{0}\".", unaryExpression.Operator.Text);
            }
        }

        private void CastOperands<T>(BinaryExpression binaryExpression, out T leftOperand, out T rightOperand)
        {
            object left = Visit(binaryExpression.LeftExpression);
            object right = Visit(binaryExpression.RightExpression);

            if(left is T && right is T)
            {
                leftOperand = (T)left;
                rightOperand = (T)right;
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
