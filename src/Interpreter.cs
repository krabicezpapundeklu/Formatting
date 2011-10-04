namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Linq;
    using System.Text;

    using Ast;

    public class Interpreter : AstVisitor
    {
        private readonly ArgumentCollection arguments;
        private readonly IFormatProvider formatProvider;
        private readonly StringBuilder formatted = new StringBuilder();

        public Interpreter(IFormatProvider formatProvider, ArgumentCollection arguments)
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
            if(argumentIndex.Index >= arguments.Count)
                throw new FormattingException(argumentIndex.Location, "Argument index is out of range.");

            return arguments[argumentIndex.Index].Value;
        }

        protected override object DoVisit(ArgumentName argumentName)
        {
            if(!arguments.Contains(argumentName.Name))
                throw new FormattingException(
                    argumentName.Location, "Argument with name \"{0}\" doesn't exist.", argumentName.Name);

            return arguments[argumentName.Name].Value;
        }

        protected override object DoVisit(BinaryExpression binaryExpression)
        {
            dynamic leftOperand = Visit(binaryExpression.LeftExpression);
            dynamic rightOperand = Visit(binaryExpression.RightExpression);

            try
            {
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

                    case ',':
                        return leftOperand || rightOperand;

                    case Token.LessOrEqual:
                        return leftOperand <= rightOperand;

                    case Token.GreaterOrEqual:
                        return leftOperand >= rightOperand;

                    case Token.And:
                        return leftOperand && rightOperand;

                    default:
                        throw new FormattingException(
                            binaryExpression.Operator.Location, "Invalid operator \"{0}\".",
                            binaryExpression.Operator.Text);
                }
            }
            catch
            {
                throw new FormattingException(
                    binaryExpression.Operator.Location,
                    "Operator \"{0}\" cannot be applied to operands of type \"{1}\" and \"{2}\".",
                    binaryExpression.Operator.Text, leftOperand.GetType(), rightOperand.GetType());
            }
        }

        protected override object DoVisit(ConditionalFormat conditionalFormat)
        {
            // to throw exception if argument is out of range
            Visit(conditionalFormat.Argument);

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
            object argument = Visit(simpleFormat.Argument);
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
                    dynamic operand = Visit(unaryExpression.Operand);

                    try
                    {
                        return -operand;
                    }
                    catch
                    {
                        throw new FormattingException(
                            unaryExpression.Operator.Location,
                            "Operator \"-\" cannot be applied to operand of type \"{0}\".", operand.GetType());
                    }

                default:
                    throw new FormattingException(
                        unaryExpression.Operator.Location, "Invalid operator \"{0}\".", unaryExpression.Operator.Text);
            }
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
