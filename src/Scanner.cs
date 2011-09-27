namespace Krabicezpapundeklu.Formatting
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

            return new TokenInfo(token, textBuilder.ToString(), new Location(tokenStart, positionInInput));
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

            switch(c)
            {
                case '<':
                    textBuilder.Append(c);
                    return Select('=', Token.LessOrEqual, '<');

                case '>':
                    textBuilder.Append(c);
                    return Select('=', Token.GreaterOrEqual, '>');

                case 'e':
                    if(positionInInput + 2 < input.Length && input[positionInInput] == 'l' &&
                        input[positionInInput + 1] == 's' && input[positionInInput + 2] == 'e' &&
                            (positionInInput + 3 == input.Length || !char.IsLetterOrDigit(input, positionInInput + 3)))
                    {
                        positionInInput += 3;
                        textBuilder.Append(input, tokenStart, positionInInput - tokenStart);
                        return Token.Else;
                    }

                    textBuilder.Append(c);
                    return c;

                default:
                    if(char.IsDigit(c))
                    {
                        while(positionInInput < input.Length && char.IsDigit(input, positionInInput))
                            positionInInput++;

                        textBuilder.Append(input, tokenStart, positionInInput - tokenStart);
                        return Token.Integer;
                    }

                    textBuilder.Append(c);
                    return c;
            }
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
