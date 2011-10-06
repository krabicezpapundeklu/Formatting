namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;

    using Ast;

    using Errors;

    public class Parser
    {
        private readonly IErrorLogger errorLogger;
        private readonly Scanner scanner;

        private TokenInfo currentTokenInfo;
        private TokenInfo nextTokenInfo;

        public Parser(Scanner scanner, IErrorLogger errorLogger)
        {
            this.scanner = Utilities.ThrowIfNull(scanner, "scanner");
            this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");

            Consume();
        }

        public FormatString Parse()
        {
            return ParseFormatString(true);
        }

        private bool Accept(int token)
        {
            if(nextTokenInfo.Token == token)
            {
                Consume();
                return true;
            }

            return false;
        }

        private void Consume()
        {
            currentTokenInfo = nextTokenInfo;
            nextTokenInfo = scanner.Scan();
        }

        private TokenInfo Expect(int token)
        {
            if(!Accept(token))
                throw SyntaxError(nextTokenInfo, "Unexpected \"{0}\".", nextTokenInfo.Text);

            return currentTokenInfo;
        }

        private Expression ParseArgumentReference()
        {
            switch(nextTokenInfo.Token)
            {
                case Token.Identifier:
                    Consume();
                    return new ArgumentName(currentTokenInfo.Location, currentTokenInfo.Text);

                case Token.Integer:
                    Consume();
                    return new ArgumentIndex(currentTokenInfo.Location, int.Parse(currentTokenInfo.Text));

                default:
                    throw SyntaxError(
                        nextTokenInfo, "Expected argument index or name, but got \"{0}\".", nextTokenInfo.Text);
            }
        }

        private Expression ParseCondition(Expression implicitOperand)
        {
            if(nextTokenInfo.Token == Token.Else)
            {
                Consume();
                return new ConstantExpression(currentTokenInfo.Location, true, "else");
            }

            Expression condition = null;

            do
            {
                Expression andExpression = null;

                do
                {
                    if(nextTokenInfo.Token == Token.Else)
                        throw new FormattingException(nextTokenInfo.Location, "\"else\" must be used alone.");

                    Operator @operator = ParseOperator();

                    if(!@operator.IsBinary)
                        throw new FormattingException(@operator.Location, "Expected binary operator.");

                    Expression rightOperand = ParseUnaryExpression();

                    var expression = new BinaryExpression(
                        Location.FromRange(@operator, rightOperand), @operator, implicitOperand, rightOperand);

                    andExpression = andExpression == null
                        ? expression
                        : new BinaryExpression(
                            Location.FromRange(andExpression, expression),
                            new Operator(Location.Unknown, Token.And, string.Empty), andExpression, expression);
                }
                while(nextTokenInfo.Token != ':' && nextTokenInfo.Token != ',');

                Accept(',');

                condition = condition == null
                    ? andExpression
                    : new BinaryExpression(
                        Location.FromRange(condition, andExpression), new Operator(currentTokenInfo.Location, ','),
                        condition, andExpression);
            }
            while(nextTokenInfo.Token != ':');

            return condition;
        }

        private ConditionalFormat ParseConditionalFormat(Expression argument)
        {
            var cases = new List<Case>();

            do
            {
                int start = Expect('{').Location.Start;
                Expression condition = ParseCondition(argument);

                scanner.State = ScannerState.ScanningText;

                Expect(':');

                FormatString formatString = ParseFormatString(false);
                int end = Expect('}').Location.End;

                scanner.State = ScannerState.ScanningTokens;

                cases.Add(new Case(new Location(start, end), condition, formatString));
            }
            while(nextTokenInfo.Token == '{');

            return new ConditionalFormat(Location.Unknown, argument, cases);
        }

        private Ast.Format ParseFormat()
        {
            scanner.State = ScannerState.ScanningTokens;

            int start = Expect('{').Location.Start;

            Expression argument = ParseArgumentReference();

            Ast.Format format;

            if(nextTokenInfo.Token == '{')
                format = ParseConditionalFormat(argument);
            else
                format = ParseSimpleFormat(argument);

            scanner.State = ScannerState.ScanningText;

            return (Ast.Format)format.Clone(new Location(start, Expect('}').Location.End));
        }

        private FormatString ParseFormatString(bool topLevel)
        {
            var items = new List<FormatStringItem>();

            while(true)
            {
                switch(nextTokenInfo.Token)
                {
                    case '{':
                        int errors = errorLogger.ErrorCount;

                        Utilities.ConvertExceptionsToLogs(errorLogger, () => items.Add(ParseFormat()));

                        if(errorLogger.ErrorCount > errors)
                        {
                            scanner.State = ScannerState.ScanningText;

                            while(nextTokenInfo.Token != '}' && nextTokenInfo.Token != Token.EndOfInput)
                                Consume();

                            break;
                        }

                        continue;

                    case '}':
                        if(!topLevel)
                            return new FormatString(Location.FromRange(items), items);
                        
                        errorLogger.LogError(nextTokenInfo.Location, "Unescaped \"}\".");
                        break;

                    case Token.EndOfInput:
                        return new FormatString(Location.FromRange(items), items);

                    case Token.Text:
                        items.Add(new Text(nextTokenInfo.Location, nextTokenInfo.Text));
                        break;

                    default:
                        // this should not happen
                        throw new InvalidOperationException(
                            string.Format("Token {0} is not valid here.", Token.ToString(nextTokenInfo.Token)));
                }

                Consume();
            }
        }

        private Operator ParseOperator()
        {
            if(Operator.IsOperator(nextTokenInfo.Token))
            {
                Consume();
                return new Operator(currentTokenInfo.Location, currentTokenInfo.Token, currentTokenInfo.Text);
            }

            throw SyntaxError(nextTokenInfo, "Unknown operator \"{0}\".", nextTokenInfo.Text);
        }

        private Expression ParsePrimaryExpression()
        {
            Consume();

            switch(currentTokenInfo.Token)
            {
                case '{':
                    Expression argument = ParseArgumentReference();
                    Expect('}');
                    return argument;

                case Token.Integer:
                    return new Integer(currentTokenInfo.Location, int.Parse(currentTokenInfo.Text));

                default:
                    throw SyntaxError(currentTokenInfo, "Expected argument, but got \"{0}\".", currentTokenInfo.Text);
            }
        }

        private SimpleFormat ParseSimpleFormat(Expression argument)
        {
            bool leftAlign;
            int width;

            if(Accept(','))
            {
                leftAlign = Accept('-');
                width = int.Parse(Expect(Token.Integer).Text);
            }
            else
            {
                leftAlign = false;
                width = 0;
            }

            scanner.State = ScannerState.ScanningText;

            return new SimpleFormat(
                Location.Unknown, argument, leftAlign, width, Accept(':')
                    ? ParseFormatString(false)
                    : FormatString.Empty);
        }

        private Expression ParseUnaryExpression()
        {
            if(Operator.IsUnaryOperator(nextTokenInfo.Token))
            {
                Operator @operator = ParseOperator();
                Expression expression = ParsePrimaryExpression();

                return new UnaryExpression(Location.FromRange(@operator, expression), @operator, expression);
            }

            return ParsePrimaryExpression();
        }

        private static FormattingException SyntaxError(TokenInfo tokenInfo, string format, params object[] arguments)
        {
            return tokenInfo.Token == Token.EndOfInput
                ? new FormattingException(tokenInfo.Location, "Unexpected end of input.")
                : new FormattingException(tokenInfo.Location, string.Format(format, arguments));
        }
    }
}
