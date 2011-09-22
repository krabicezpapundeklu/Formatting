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
                format = ParseRestOfConditionalFormat(argumentIndex);
            }
            else
            {
                format = ParseRestOfSimpleFormat(argumentIndex);
            }

            scanner.State = ScannerState.ScanningText;

            format.Start = start;
            format.End = Expect('}').End;

            return format;
        }

        private ConditionalFormat ParseRestOfConditionalFormat(ArgumentIndex argumentIndex)
        {
            throw new NotImplementedException();
        }

        private SimpleFormat ParseRestOfSimpleFormat(ArgumentIndex argumentIndex)
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
    }
}
