﻿namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    public class Scanner
    {
        private readonly string input;
        private readonly StringBuilder textBuilder = new StringBuilder();

        private int positionInInput;
        private int tokenStart;

        public Scanner(string input)
        {
            if(input == null)
                throw new ArgumentNullException("input");

            this.input = input;

            State = ScannerState.ScanningText;
        }

        public ScannerState State { get; set; }

        public TokenInfo Scan()
        {
            textBuilder.Clear();

            if(State == ScannerState.ScanningTokens)
                SkipWhiteSpace();

            int token;

            if((tokenStart = positionInInput) < input.Length)
                switch(State)
                {
                    case ScannerState.ScanningText:
                        token = ScanText();
                        break;

                    case ScannerState.ScanningTokens:
                        token = ScanTokens();
                        break;

                    default:
                        // this should not happen
                        throw new InvalidOperationException(string.Format("State \"{0}\" is not supported.", State));
                }
            else
                token = Token.EndOfInput;

            return new TokenInfo(new Location(tokenStart, positionInInput), token, textBuilder.ToString());
        }

        private int ScanText()
        {
            do
            {
                char c = input[positionInInput++];

                switch(c)
                {
                    case '{':
                    case '}':
                        if(textBuilder.Length == 0)
                        {
                            textBuilder.Append(c);
                            return c;
                        }

                        positionInInput--;
                        return Token.Text;

                    case '\\':
                        if(positionInInput == input.Length)
                            throw new FormattingException(
                                new Location(positionInInput, positionInInput), "Unexpected end of input.");

                        if(!EscapeHelpers.MustBeEscaped(c = input[positionInInput++]))
                            throw new FormattingException(
                                new Location(positionInInput - 2, positionInInput), "\"{0}\" cannot be escaped.", c);

                        break;
                }

                textBuilder.Append(c);
            }
            while(positionInInput < input.Length);

            return Token.Text;
        }

        private int ScanTokens()
        {
            char c = input[positionInInput++];

            textBuilder.Append(c);

            switch(c)
            {
                case '<':
                    return Select('=', Token.LessOrEqual, '<');

                case '>':
                    return Select('=', Token.GreaterOrEqual, '>');

                default:
                    if(char.IsDigit(c))
                    {
                        ScanWhile(char.IsDigit);
                        return Token.Integer;
                    }

                    if(char.IsLetter(c))
                    {
                        ScanWhile(char.IsLetter);

                        return textBuilder.ToString() == "else"
                            ? Token.Else
                            : Token.Identifier;
                    }

                    return c;
            }
        }

        private void ScanWhile(Func<string, int, bool> predicate)
        {
            while(positionInInput < input.Length && predicate(input, positionInInput))
                positionInInput++;

            textBuilder.Append(input, tokenStart + 1, positionInInput - tokenStart - 1);
        }

        private int Select(char following, int ifFollows, int ifDoesNotFollow)
        {
            if(positionInInput < input.Length && input[positionInInput] == following)
            {
                textBuilder.Append(following);
                positionInInput++;
                return ifFollows;
            }

            return ifDoesNotFollow;
        }

        private void SkipWhiteSpace()
        {
            while(positionInInput < input.Length && char.IsWhiteSpace(input, positionInInput))
                ++positionInInput;
        }
    }
}
