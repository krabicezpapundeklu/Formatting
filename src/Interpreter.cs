namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using Krabicezpapundeklu.Formatting.Ast;
    using Krabicezpapundeklu.Formatting.Errors;

    public class Interpreter : AstVisitor
    {
        #region Constants and Fields

        private static readonly object Error = new object();

        private readonly ArgumentCollection arguments;

        private readonly IErrorLogger errorLogger;

        private readonly IFormatProvider formatProvider;

        private readonly StringBuilder formatted = new StringBuilder();

        #endregion

        #region Constructors and Destructors

        public Interpreter(IFormatProvider formatProvider, ArgumentCollection arguments, IErrorLogger errorLogger)
        {
            this.formatProvider = formatProvider;
            this.arguments = Utilities.ThrowIfNull(arguments, "arguments");
            this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");
        }

        #endregion

        #region Public Methods

        public string Evaluate(AstNode ast)
        {
            return this.Visit<string>(ast);
        }

        #endregion

        #region Methods

        protected override object Default(AstNode node)
        {
            return null;
        }

        protected override object DoVisit(ArgumentIndex argumentIndex)
        {
            if (argumentIndex.Index >= this.arguments.Count)
            {
                this.errorLogger.LogError(
                    argumentIndex.Location, string.Format("Argument index {0} is out of range.", argumentIndex.Index));
                return Error;
            }

            return this.arguments[argumentIndex.Index].Value;
        }

        protected override object DoVisit(ArgumentName argumentName)
        {
            if (!this.arguments.Contains(argumentName.Name))
            {
                this.errorLogger.LogError(
                    argumentName.Location, string.Format("Argument with name \"{0}\" doesn't exist.", argumentName.Name));

                return Error;
            }

            return this.arguments[argumentName.Name].Value;
        }

        protected override object DoVisit(BinaryExpression binaryExpression)
        {
            dynamic leftOperand = Visit(binaryExpression.LeftExpression);
            dynamic rightOperand = Visit(binaryExpression.RightExpression);

            if ((object)leftOperand == Error || (object)rightOperand == Error)
            {
                return Error;
            }

            try
            {
                switch (binaryExpression.Operator.Token)
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
                        this.errorLogger.LogError(
                            binaryExpression.Operator.Location,
                            string.Format("Invalid operator \"{0}\".", binaryExpression.Operator.Text));

                        return Error;
                }
            }
            catch
            {
                this.errorLogger.LogError(
                    binaryExpression.Operator.Location,
                    string.Format(
                        "Operator \"{0}\" cannot be applied to operands of type \"{1}\" and \"{2}\".",
                        binaryExpression.Operator.Text,
                        leftOperand.GetType(),
                        rightOperand.GetType()));

                return Error;
            }
        }

        protected override object DoVisit(ConditionalFormat conditionalFormat)
        {
            // to catch errors, ignore result
            Visit(conditionalFormat.Argument);

            foreach (Case @case in conditionalFormat.Cases)
            {
                object conditionResult = Visit(@case.Condition);

                if (conditionResult == Error)
                {
                    // visit to catch errors, but ignore result
                    Visit(@case.FormatString);
                    continue;
                }

                if ((bool)conditionResult)
                {
                    return Visit(@case.FormatString);
                }
            }

            return string.Empty;
        }

        protected override object DoVisit(ConstantExpression constantExpression)
        {
            return constantExpression.Value;
        }

        protected override object DoVisit(FormatString formatString)
        {
            int originalLength = this.formatted.Length;

            foreach (FormatStringItem item in formatString.Items)
            {
                this.formatted.Append(Visit(item));
            }

            string formattedString = this.formatted.ToString(originalLength, this.formatted.Length - originalLength);

            this.formatted.Length = originalLength;

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

            if (argument == Error || format == Error)
            {
                return null;
            }

            string formattedArgument = this.Format(argument, (string)format);

            int padding = simpleFormat.Width - formattedArgument.Length;

            if (padding > 0)
            {
                if (simpleFormat.LeftAlign)
                {
                    this.formatted.Append(formattedArgument);
                    this.formatted.Append(' ', padding);
                }
                else
                {
                    this.formatted.Append(' ', padding);
                    this.formatted.Append(formattedArgument);
                }
            }
            else
            {
                this.formatted.Append(formattedArgument);
            }

            return null;
        }

        protected override object DoVisit(Text text)
        {
            return text.Value;
        }

        protected override object DoVisit(UnaryExpression unaryExpression)
        {
            switch (unaryExpression.Operator.Token)
            {
                case '-':
                    dynamic operand = Visit(unaryExpression.Operand);

                    try
                    {
                        return -operand;
                    }
                    catch
                    {
                        this.errorLogger.LogError(
                            unaryExpression.Operator.Location,
                            string.Format(
                                "Operator \"-\" cannot be applied to operand of type \"{0}\".", operand.GetType()));

                        return Error;
                    }

                default:
                    this.errorLogger.LogError(
                        unaryExpression.Operator.Location,
                        string.Format("Invalid operator \"{0}\".", unaryExpression.Operator.Text));

                    return Error;
            }
        }

        private string Format(object argument, string format)
        {
            if (argument == null)
            {
                return string.Empty;
            }

            ICustomFormatter formatter = this.formatProvider == null
                                             ? null
                                             : this.formatProvider.GetFormat(typeof(ICustomFormatter)) as
                                               ICustomFormatter;

            if (formatter != null)
            {
                return formatter.Format(format, argument, this.formatProvider);
            }

            if (argument is IFormattable)
            {
                return ((IFormattable)argument).ToString(format, this.formatProvider);
            }

            return argument.ToString();
        }

        #endregion
    }
}