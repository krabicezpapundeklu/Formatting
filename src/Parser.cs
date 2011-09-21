namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Linq;

    using Ast;

    public class Parser
    {
        private struct TokenInfo
        {
            public int token;
            public string text;
            public int start;
            public int end;
        }

        private readonly IScanner scanner;

        private TokenInfo currentTokenInfo;
        private TokenInfo nextTokenInfo;

        public Parser(IScanner scanner)
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
            if(nextTokenInfo.token == token)
            {
                Consume();
                return true;
            }

            return false;
        }

        private void Consume()
        {
            currentTokenInfo = nextTokenInfo;

            scanner.Scan();

            nextTokenInfo.token = scanner.Token;
            nextTokenInfo.text = scanner.Text;
            nextTokenInfo.start = scanner.Start;
            nextTokenInfo.end = scanner.End;
        }

        private void Expect(int token)
        {
            if(!Accept(token))
            {
                throw new Exception("invalid token"); // TODO
            }
        }

        private FormatString ParseFormatString()
        {
            var formatString = new FormatString();

            while(true)
            {
                switch(nextTokenInfo.token)
                {
                    case '{':
                        formatString.Items.Add(ParseFormat());
                        continue;

                    case '}':
                    case Token.EndOfInput:
                        if(formatString.Items.Count > 0)
                        {
                            formatString.Start = formatString.Items[0].Start;
                            formatString.End = formatString.Items.Last().End;
                        }

                        return formatString;

                    case Token.Text:
                        formatString.Items.Add(
                            new Text(nextTokenInfo.text)
                            {
                                Start = nextTokenInfo.start, End = nextTokenInfo.end
                            });

                        break;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Token {0} is not valid here.", Token.ToString(nextTokenInfo.token)));
                }

                Consume();
            }
        }

        private Format ParseFormat()
        {
            scanner.State = ScannerState.ScanningTokens;

            try
            {
                Expect('{');

                int start = currentTokenInfo.start;

                Expect(Token.Integer);

                var argumentIndex =
                    new ArgumentIndex(int.Parse(currentTokenInfo.text))
                    {
                        Start = currentTokenInfo.start,
                        End = currentTokenInfo.end
                    };

                bool leftAlign;
                int width;

                if(Accept(','))
                {
                    leftAlign = Accept('-');
                    Expect(Token.Integer);
                    width = int.Parse(currentTokenInfo.text);
                }
                else
                {
                    leftAlign = false;
                    width = 0;
                }

                scanner.State = ScannerState.ScanningText;

                var format =
                    new SimpleFormat(argumentIndex, leftAlign, width,
                        Accept(':') ? ParseFormatString() : new FormatString());

                Expect('}');

                format.Start = start;
                format.End = currentTokenInfo.end;

                return format;
            }
            finally
            {
                scanner.State = ScannerState.ScanningText;
            }
        }
    }
}
