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
                throw new ArgumentNullException("scanner");

            this.scanner = scanner;

            Consume();
        }

        public FormatString Parse()
        {
            FormatString formatString = ParseFormatString();

            if(!Accept(Token.EndOfInput))
                throw new FormattingException(nextTokenInfo.Location, "Unescaped \"}}\".");

            return formatString;
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
            {
                if(nextTokenInfo.Token == Token.EndOfInput)
                    throw UnexpectedEndOfInput(nextTokenInfo.Location);

                throw new FormattingException(nextTokenInfo.Location, "Unexpected \"{0}\".", nextTokenInfo.Text);
            }

            return currentTokenInfo;
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
                        : new BinaryExpression(new Operator(Token.And, string.Empty), andExpression, expression);
                }
                while(nextTokenInfo.Token != ':' && nextTokenInfo.Token != ',');

                Accept(',');

                condition = condition == null
                    ? andExpression
                    : new BinaryExpression(new Operator(currentTokenInfo.Location, ','), condition, andExpression);
            }
            while(nextTokenInfo.Token != ':');

            return condition;
        }

        private ConditionalFormat ParseConditionalFormat(ArgumentIndex argumentIndex)
        {
            var cases = new List<Case>();

            do
            {
                int start = Expect('{').Location.Start;
                Expression condition = ParseCondition(argumentIndex);

                scanner.State = ScannerState.ScanningText;

                Expect(':');

                FormatString formatString = ParseFormatString();
                int end = Expect('}').Location.End;

                scanner.State = ScannerState.ScanningTokens;

                cases.Add(new Case(new Location(start, end), condition, formatString));
            }
            while(nextTokenInfo.Token == '{');

            return new ConditionalFormat(argumentIndex, cases);
        }

        private Ast.Format ParseFormat()
        {
            scanner.State = ScannerState.ScanningTokens;

            int start = Expect('{').Location.Start;
            int index = int.Parse(Expect(Token.Integer).Text);

            var argumentIndex = new ArgumentIndex(currentTokenInfo.Location, index);

            Ast.Format format;

            if(nextTokenInfo.Token == '{')
                format = ParseConditionalFormat(argumentIndex);
            else
                format = ParseSimpleFormat(argumentIndex);

            scanner.State = ScannerState.ScanningText;

            return (Ast.Format)format.Clone(new Location(start, Expect('}').Location.End));
        }

        private FormatString ParseFormatString()
        {
            var items = new List<FormatStringItem>();

            while(true)
            {
                switch(nextTokenInfo.Token)
                {
                    case '{':
                        items.Add(ParseFormat());
                        continue;

                    case '}':
                    case Token.EndOfInput:
                        return items.Count == 0
                            ? new FormatString(new Location(0, 0), items)
                            : new FormatString(items);

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

            if(nextTokenInfo.Token == Token.EndOfInput)
                throw UnexpectedEndOfInput(nextTokenInfo.Location);

            throw new FormattingException(nextTokenInfo.Location, "Unknown operator \"{0}\".", nextTokenInfo.Text);
        }

        private Expression ParsePrimaryExpression()
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
                    if(currentTokenInfo.Token == Token.EndOfInput)
                        throw UnexpectedEndOfInput(currentTokenInfo.Location);

                    throw new FormattingException(
                        currentTokenInfo.Location, "Expected argument, but got \"{0}\".", currentTokenInfo.Text);
            }
        }

        private SimpleFormat ParseSimpleFormat(ArgumentIndex argumentIndex)
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
                argumentIndex, leftAlign, width, Accept(':')
                    ? ParseFormatString()
                    : FormatString.Empty);
        }

        private Expression ParseUnaryExpression()
        {
            return nextTokenInfo.Token == '-'
                ? new UnaryExpression(ParseOperator(), ParsePrimaryExpression())
                : ParsePrimaryExpression();
        }

        private static FormattingException UnexpectedEndOfInput(Location location)
        {
            return new FormattingException(location, "Unexpected end of input.");
        }
    }
}
