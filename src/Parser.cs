namespace Krabicezpapundeklu.Formatting
{
    using System;
    
    using Ast;

    public class Parser
    {
        private readonly Scanner scanner;
        private readonly TokenInfo currentTokenInfo = new TokenInfo();
        
        public Parser(Scanner scanner)
        {
            if(scanner == null)
            {
                throw new ArgumentNullException("scanner");
            }

            this.scanner = scanner;

            Consume();
        }

        public FormatString Parse()
        {
            var formatString = ParseFormatString();
            Expect(Token.EndOfInput);

            return formatString;
        }

        private bool Accept(int token)
        {
            if(scanner.Token == token)
            {
                Consume();
                return true;
            }

            return false;
        }

        private void Consume()
        {
            currentTokenInfo.AssignFrom(scanner);
            scanner.Scan();
        }

        private TokenInfo Expect(int token)
        {
            if(!Accept(token))
            {
                throw new FormatException("invalid token"); // TODO
            }

            return currentTokenInfo;
        }

        private IExpression ParseCondition(IExpression leftOperand)
        {
            IExpression condition = null;

            do
            {
                var binaryOperator = ParseOperator();
                var expression = new BinaryExpression(binaryOperator, leftOperand, ParseUnaryExpression());
    
                condition = condition == null
                    ? expression
                    : new BinaryExpression(new Operator(Token.And), condition, expression);
            }
            while (scanner.Token != ':');

            return condition;
        }

        private FormatString ParseFormatString()
        {
            var formatString = new FormatString();

            while(true)
            {
                switch(scanner.Token)
                {
                    case '{':
                        formatString.Items.Add(ParseFormat());
                        continue;

                    case '}':
                    case Token.EndOfInput:
                        return formatString;

                    case Token.Text:
                        formatString.Items.Add(
                            new Text(scanner.Text){Start = scanner.Start, End = scanner.End});

                        break;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Token {0} is not valid here.", Token.ToString(scanner.Token)));
                }

                Consume();
            }
        }

        private Format ParseFormat()
        {
            scanner.State = ScannerState.ScanningTokens;

            int start = Expect('{').Start;

            var argumentIndex =
                new ArgumentIndex(int.Parse(Expect(Token.Integer).Text))
                    {Start = currentTokenInfo.Start, End = currentTokenInfo.End};

            Format format;

            if(scanner.Token == '{')
            {
                format = ParseConditionalFormat(argumentIndex);
            }
            else
            {
                format = ParseSimpleFormat(argumentIndex);
            }

            scanner.State = ScannerState.ScanningText;

            format.Start = start;
            format.End = Expect('}').End;

            return format;
        }

        private Operator ParseOperator()
        {
            Consume();

            switch (currentTokenInfo.Token)
            {
                case '-':
                case '=':
                case '!':
                case '>':
                case '<':
                    return new Operator(currentTokenInfo.Token)
                        {Start = currentTokenInfo.Start, End = currentTokenInfo.End};

                default:
                    // TODO
                    throw new FormatException(string.Format("Unknown operator \"{0}\".", currentTokenInfo.Text));
            }
        }

        private IExpression ParsePrimaryExpression()
        {
            Consume();

            switch(currentTokenInfo.Token)
            {
                case '{':
                    int start = currentTokenInfo.Start;

                    return new ArgumentIndex(int.Parse(Expect(Token.Integer).Text))
                        {Start = start, End = Expect('}').End};

                case Token.Integer:
                    return new Integer(int.Parse(currentTokenInfo.Text))
                        {Start = currentTokenInfo.Start, End = currentTokenInfo.End};

                default:
                    // TODO
                    throw new FormatException(
                        string.Format("Expected argument, but got \"{0}\".", currentTokenInfo.Text));
            }
        }

        private ConditionalFormat ParseConditionalFormat(ArgumentIndex argumentIndex)
        {
            var conditionalFormat = new ConditionalFormat(argumentIndex);

            do
            {
                int start = Expect('{').Start;
                var condition = ParseCondition(argumentIndex);

                scanner.State = ScannerState.ScanningText;

                Expect(':');

                var formatString = ParseFormatString();
                int end = Expect('}').End;

                scanner.State = ScannerState.ScanningTokens;

                conditionalFormat.Cases.Add(
                    new Case(condition, formatString) { Start = start, End = end });
            }
            while (scanner.Token == '{');

            return conditionalFormat;
        }

        private SimpleFormat ParseSimpleFormat(ArgumentIndex argumentIndex)
        {
            bool leftAlign;
            int width;

            if (Accept(','))
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

            return new SimpleFormat(argumentIndex, leftAlign, width,
                Accept(':') ? ParseFormatString() : new FormatString());
        }

        private IExpression ParseUnaryExpression()
        {
            return scanner.Token == '-'
                ? new UnaryExpression(ParseOperator(), ParsePrimaryExpression())
                : ParsePrimaryExpression();
        }
    }
}
