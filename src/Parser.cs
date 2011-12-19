namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;

    using Krabicezpapundeklu.Formatting.Ast;
    using Krabicezpapundeklu.Formatting.Errors;
    using System.Diagnostics.CodeAnalysis;

    public class Parser
    {
        #region Constants and Fields

        private readonly IErrorLogger errorLogger;

        private readonly Scanner scanner;

        private TokenInfo currentTokenInfo;

        private TokenInfo nextTokenInfo;

        #endregion

        #region Constructors and Destructors

        public Parser(Scanner scanner, IErrorLogger errorLogger)
        {
            this.scanner = Utilities.ThrowIfNull(scanner, "scanner");
            this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");

            this.Consume();
        }

        #endregion

        #region Public Methods

        public FormatString Parse()
        {
            return this.ParseFormatString(0);
        }

        #endregion

        #region Methods

        private static FormatString CreateFormatString(int start, ICollection<FormatStringItem> items)
        {
            return items.Count == 0
                       ? new FormatString(new Location(start, start), items) // to have "known" location
                       : new FormatString(Location.FromRange(items), items);
        }

        private static FormattingException SyntaxError(TokenInfo tokenInfo, string format, params object[] arguments)
        {
            return tokenInfo.Token == Token.EndOfInput
                       ? new FormattingException(tokenInfo.Location, "Unexpected end of input.")
                       : new FormattingException(tokenInfo.Location, Utilities.InvariantFormat(format, arguments));
        }

        private bool Accept(int token)
        {
            if (this.nextTokenInfo.Token == token)
            {
                this.Consume();
                return true;
            }

            return false;
        }

        private void Consume()
        {
            this.currentTokenInfo = this.nextTokenInfo;
            this.nextTokenInfo = this.scanner.Scan();
        }

        private TokenInfo Expect(int token)
        {
            if (!this.Accept(token))
            {
                throw SyntaxError(this.nextTokenInfo, "Unexpected \"{0}\".", this.nextTokenInfo.Text);
            }

            return this.currentTokenInfo;
        }

        private Expression ParseArgumentReference()
        {
            switch (this.nextTokenInfo.Token)
            {
                case Token.Identifier:
                    this.Consume();
                    return new ArgumentName(this.currentTokenInfo.Location, this.currentTokenInfo.Text);

                case Token.Integer:
                    this.Consume();
                    return new ArgumentIndex(this.currentTokenInfo.Location, int.Parse(this.currentTokenInfo.Text));

                default:
                    throw SyntaxError(
                        this.nextTokenInfo, "Expected argument index or name, but got \"{0}\".", this.nextTokenInfo.Text);
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Krabicezpapundeklu.Formatting.Ast.ConstantExpression.#ctor(Krabicezpapundeklu.Formatting.Location,System.Object,System.String)")]
        private Expression ParseCondition(Expression implicitOperand)
        {
            if (this.nextTokenInfo.Token == Token.Else)
            {
                this.Consume();
                return new ConstantExpression(this.currentTokenInfo.Location, true, "else");
            }

            Expression condition = null;

            do
            {
                Expression andExpression = null;

                do
                {
                    if (this.nextTokenInfo.Token == Token.Else)
                    {
                        throw new FormattingException(this.nextTokenInfo.Location, "\"else\" must be used alone.");
                    }

                    Operator @operator = this.ParseOperator();

                    if (!@operator.IsBinary)
                    {
                        throw new FormattingException(@operator.Location, "Expected binary operator.");
                    }

                    Expression rightOperand = this.ParseUnaryExpression();

                    var expression = new BinaryExpression(
                        Location.FromRange(@operator, rightOperand), @operator, implicitOperand, rightOperand);

                    andExpression = andExpression == null
                                        ? expression
                                        : new BinaryExpression(
                                              Location.FromRange(andExpression, expression),
                                              new Operator(Location.Unknown, Token.And, string.Empty),
                                              andExpression,
                                              expression);
                }
                while (this.nextTokenInfo.Token != ':' && this.nextTokenInfo.Token != ',');

                this.Accept(',');

                condition = condition == null
                                ? andExpression
                                : new BinaryExpression(
                                      Location.FromRange(condition, andExpression),
                                      new Operator(this.currentTokenInfo.Location, ','),
                                      condition,
                                      andExpression);
            }
            while (this.nextTokenInfo.Token != ':');

            return condition;
        }

        private ConditionalFormat ParseConditionalFormat(Expression argument)
        {
            var cases = new List<Case>();

            do
            {
                int start = this.Expect('{').Location.Start;
                Expression condition = this.ParseCondition(argument);

                this.scanner.State = ScannerState.ScanningText;

                this.Expect(':');

                FormatString formatString = this.ParseFormatString(this.nextTokenInfo.Location.Start);
                int end = this.Expect('}').Location.End;

                this.scanner.State = ScannerState.ScanningTokens;

                cases.Add(new Case(new Location(start, end), condition, formatString));
            }
            while (this.nextTokenInfo.Token == '{');

            return new ConditionalFormat(Location.Unknown, argument, cases);
        }

        private Ast.Format ParseFormat()
        {
            this.scanner.State = ScannerState.ScanningTokens;

            int start = this.Expect('{').Location.Start;

            Expression argument = this.ParseArgumentReference();

            Ast.Format format;

            if (this.nextTokenInfo.Token == '{')
            {
                format = this.ParseConditionalFormat(argument);
            }
            else
            {
                format = this.ParseSimpleFormat(argument);
            }

            this.scanner.State = ScannerState.ScanningText;

            return (Ast.Format)format.Clone(new Location(start, this.Expect('}').Location.End));
        }

        private FormatString ParseFormatString(int start)
        {
            var items = new List<FormatStringItem>();

            while (true)
            {
                switch (this.nextTokenInfo.Token)
                {
                    case '{':
                        try
                        {
                            items.Add(this.ParseFormat());
                        }
                        catch (FormattingException e)
                        {
                            foreach (Error error in e.Errors)
                            {
                                this.errorLogger.LogError(error);
                            }

                            this.scanner.State = ScannerState.ScanningText;

                            while (this.nextTokenInfo.Token != '}' && this.nextTokenInfo.Token != Token.EndOfInput)
                            {
                                this.Consume();
                            }

                            break;
                        }

                        continue;

                    case '}':
                        if (start > 0)
                        {
                            return CreateFormatString(start, items);
                        }

                        this.errorLogger.LogError(this.nextTokenInfo.Location, "Unescaped \"}\".");
                        break;

                    case Token.EndOfInput:
                        return CreateFormatString(start, items);

                    case Token.Text:
                        items.Add(new Text(this.nextTokenInfo.Location, this.nextTokenInfo.Text));
                        break;

                    default:
                        // this should not happen
                        throw new InvalidOperationException(
                            Utilities.InvariantFormat("Token {0} is not valid here.", Token.ToString(this.nextTokenInfo.Token)));
                }

                this.Consume();
            }
        }

        private Operator ParseOperator()
        {
            if (Operator.IsOperator(this.nextTokenInfo.Token))
            {
                this.Consume();

                return new Operator(
                    this.currentTokenInfo.Location, this.currentTokenInfo.Token, this.currentTokenInfo.Text);
            }

            throw SyntaxError(this.nextTokenInfo, "Unknown operator \"{0}\".", this.nextTokenInfo.Text);
        }

        private Expression ParsePrimaryExpression()
        {
            this.Consume();

            switch (this.currentTokenInfo.Token)
            {
                case '{':
                    Expression argument = this.ParseArgumentReference();
                    this.Expect('}');
                    return argument;

                case Token.Integer:
                    return new Integer(this.currentTokenInfo.Location, int.Parse(this.currentTokenInfo.Text));

                default:
                    throw SyntaxError(
                        this.currentTokenInfo, "Expected argument, but got \"{0}\".", this.currentTokenInfo.Text);
            }
        }

        private SimpleFormat ParseSimpleFormat(Expression argument)
        {
            bool leftAlign;
            int width;

            if (this.Accept(','))
            {
                leftAlign = this.Accept('-');
                width = int.Parse(this.Expect(Token.Integer).Text);
            }
            else
            {
                leftAlign = false;
                width = 0;
            }

            this.scanner.State = ScannerState.ScanningText;

            return new SimpleFormat(
                Location.Unknown,
                argument,
                leftAlign,
                width,
                this.Accept(':') ? this.ParseFormatString(this.currentTokenInfo.Location.Start) : FormatString.Empty);
        }

        private Expression ParseUnaryExpression()
        {
            if (Operator.IsUnaryOperator(this.nextTokenInfo.Token))
            {
                Operator @operator = this.ParseOperator();
                Expression expression = this.ParsePrimaryExpression();

                return new UnaryExpression(Location.FromRange(@operator, expression), @operator, expression);
            }

            return this.ParsePrimaryExpression();
        }

        #endregion
    }
}