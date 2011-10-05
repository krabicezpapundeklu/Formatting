namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using Ast;

    using Errors;

    public class Interpreter : AstVisitor
    {
        private static readonly object Error = new object();

        private readonly ArgumentCollection arguments;
        private readonly IErrorLogger errorLogger;
        private readonly IFormatProvider formatProvider;
        private readonly StringBuilder formatted = new StringBuilder();

        public Interpreter(IFormatProvider formatProvider, ArgumentCollection arguments, IErrorLogger errorLogger)
        {
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            if(errorLogger == null)
                throw new ArgumentNullException("errorLogger");

            this.formatProvider = formatProvider;
            this.arguments = arguments;
            this.errorLogger = errorLogger;
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
            {
                errorLogger.LogError(argumentIndex.Location, "Argument index {0} is out of range.", argumentIndex.Index);
                return Error;
            }

            return arguments[argumentIndex.Index].Value;
        }

        protected override object DoVisit(ArgumentName argumentName)
        {
            if(!arguments.Contains(argumentName.Name))
            {
                errorLogger.LogError(
                    argumentName.Location, "Argument with name \"{0}\" doesn't exist.", argumentName.Name);

                return Error;
            }

            return arguments[argumentName.Name].Value;
        }

        protected override object DoVisit(BinaryExpression binaryExpression)
        {
            dynamic leftOperand = Visit(binaryExpression.LeftExpression);
            dynamic rightOperand = Visit(binaryExpression.RightExpression);

            if((object)leftOperand == Error || (object)rightOperand == Error)
                return Error;

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
                        errorLogger.LogError(
                            binaryExpression.Operator.Location, "Invalid operator \"{0}\".",
                            binaryExpression.Operator.Text);

                        return Error;
                }
            }
            catch
            {
                errorLogger.LogError(
                    binaryExpression.Operator.Location,
                    "Operator \"{0}\" cannot be applied to operands of type \"{1}\" and \"{2}\".",
                    binaryExpression.Operator.Text, leftOperand.GetType(), rightOperand.GetType());

                return Error;
            }
        }

        protected override object DoVisit(ConditionalFormat conditionalFormat)
        {
            // to catch errors, ignore result
            Visit(conditionalFormat.Argument);

            foreach(Case @case in conditionalFormat.Cases)
            {
                object conditionResult = Visit(@case.Condition);

                if(conditionResult == Error)
                {
                    // visit to catch errors, but ignore result
                    Visit(@case.FormatString);
                    continue;
                }

                if((bool)conditionResult)
                    return Visit(@case.FormatString);
            }

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
            object format = Visit(simpleFormat.FormatString);

            if(argument == Error || format == Error)
                return null;

            string formattedArgument = Format(argument, (string)format);

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
                        errorLogger.LogError(
                            unaryExpression.Operator.Location,
                            "Operator \"-\" cannot be applied to operand of type \"{0}\".", operand.GetType());

                        return Error;
                    }

                default:
                    errorLogger.LogError(
                        unaryExpression.Operator.Location, "Invalid operator \"{0}\".", unaryExpression.Operator.Text);
                    return Error;
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
