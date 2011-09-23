﻿namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    
    using Ast;

    public class Parser
    {
        private readonly Scanner scanner;
        
        private TokenInfo currentTokenInfo;
        private TokenInfo nextTokenInfo;

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
            if (nextTokenInfo.Token == token)
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
            {
                throw new FormatException("invalid token"); // TODO
            }

            return currentTokenInfo;
        }

        private IExpression ParseCondition(IExpression implicitOperand)
        {
            IExpression condition = null;

            do
            {
                IExpression andExpression = null;

                do
                {
                    var binaryOperator = ParseOperator();
                    var expression = new BinaryExpression(binaryOperator, implicitOperand, ParseUnaryExpression());

                    andExpression = andExpression == null
                        ? expression
                        : new BinaryExpression(new Operator(Location.Unknown, Token.And), andExpression, expression);
                }
                while (nextTokenInfo.Token != ':' && nextTokenInfo.Token != ',');

                Accept(',');

                condition = condition == null
                    ? andExpression
                    : new BinaryExpression(new Operator(currentTokenInfo.Location, ','), condition, andExpression);
            }
            while (nextTokenInfo.Token != ':');
            
            return condition;
        }

        private FormatString ParseFormatString()
        {
            var items = new List<IFormatStringItem>();

            while(true)
            {
                switch (nextTokenInfo.Token)
                {
                    case '{':
                        items.Add(ParseFormat());
                        continue;

                    case '}':
                    case Token.EndOfInput:
                        return new FormatString(items);

                    case Token.Text:
                        items.Add(new Text(nextTokenInfo.Location, nextTokenInfo.Text));
                        break;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Token {0} is not valid here.", Token.ToString(nextTokenInfo.Token)));
                }

                Consume();
            }
        }

        private Format ParseFormat()
        {
            scanner.State = ScannerState.ScanningTokens;

            int start = Expect('{').Location.Start;
            int index = int.Parse(Expect(Token.Integer).Text);

            var argumentIndex = new ArgumentIndex(currentTokenInfo.Location, index);

            Format format;

            if (nextTokenInfo.Token == '{')
            {
                format = ParseConditionalFormat(argumentIndex);
            }
            else
            {
                format = ParseSimpleFormat(argumentIndex);
            }

            scanner.State = ScannerState.ScanningText;

            return format.Clone(new Location(start, Expect('}').Location.End));
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
                    return new Operator(currentTokenInfo.Location, currentTokenInfo.Token);

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
                    int start = currentTokenInfo.Location.Start;
                    int index = int.Parse(Expect(Token.Integer).Text);
                    int end = Expect('}').Location.End;

                    return new ArgumentIndex(new Location(start, end), index); 

                case Token.Integer:
                    return new Integer(currentTokenInfo.Location, int.Parse(currentTokenInfo.Text));

                default:
                    // TODO
                    throw new FormatException(
                        string.Format("Expected argument, but got \"{0}\".", currentTokenInfo.Text));
            }
        }

        private ConditionalFormat ParseConditionalFormat(ArgumentIndex argumentIndex)
        {
            var cases = new List<Case>();

            do
            {
                int start = Expect('{').Location.Start;
                var condition = ParseCondition(argumentIndex);

                scanner.State = ScannerState.ScanningText;

                Expect(':');

                var formatString = ParseFormatString();
                int end = Expect('}').Location.End;

                scanner.State = ScannerState.ScanningTokens;

                cases.Add(new Case(new Location(start, end),  condition, formatString));
            }
            while (nextTokenInfo.Token == '{');

            return new ConditionalFormat(Location.Unknown, argumentIndex, cases);
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

            return new SimpleFormat(Location.Unknown, argumentIndex, leftAlign, width,
                Accept(':') ? ParseFormatString() : FormatString.Empty);
        }

        private IExpression ParseUnaryExpression()
        {
            return nextTokenInfo.Token == '-'
                ? new UnaryExpression(ParseOperator(), ParsePrimaryExpression())
                : ParsePrimaryExpression();
        }
    }
}
