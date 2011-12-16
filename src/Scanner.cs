namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using Krabicezpapundeklu.Formatting.Errors;

    public class Scanner
    {
        #region Constants and Fields

        private readonly IErrorLogger errorLogger;

        private readonly string input;

        private readonly StringBuilder textBuilder = new StringBuilder();

        private int positionInInput;

        private int tokenStart;

        #endregion

        #region Constructors and Destructors

        public Scanner(string input, IErrorLogger errorLogger)
        {
            this.input = Utilities.ThrowIfNull(input, "input");
            this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");

            this.State = ScannerState.ScanningText;
        }

        #endregion

        #region Public Properties

        public ScannerState State { get; set; }

        #endregion

        #region Public Methods

        public static bool IsValidIdentifier(string text)
        {
            Utilities.ThrowIfNull(text, "text");

            if (text.Length == 0)
            {
                return false;
            }

            if (!IsValidIdentifierStartCharacter(text[0]))
            {
                return false;
            }

            for (int i = 1; i < text.Length; ++i)
            {
                if (!IsValidIdentifierCharacter(text, i))
                {
                    return false;
                }
            }

            return true;
        }

        public TokenInfo Scan()
        {
            this.textBuilder.Clear();

            if (this.State == ScannerState.ScanningTokens)
            {
                this.SkipWhile(char.IsWhiteSpace);
            }

            int token;

            if ((this.tokenStart = this.positionInInput) < this.input.Length)
            {
                switch (this.State)
                {
                    case ScannerState.ScanningText:
                        token = this.ScanText();
                        break;

                    case ScannerState.ScanningTokens:
                        token = this.ScanTokens();
                        break;

                    default:
                        // this should not happen
                        throw new InvalidOperationException(
                            Utilities.InvariantFormat("State \"{0}\" is not supported.", this.State));
                }
            }
            else
            {
                token = Token.EndOfInput;
            }

            return new TokenInfo(
                new Location(this.tokenStart, this.positionInInput), token, this.textBuilder.ToString());
        }

        #endregion

        #region Methods

        private static bool IsValidIdentifierCharacter(string text, int index)
        {
            return text[index] == '_' || text[index] == '-' || text[index] == '.' || char.IsLetterOrDigit(text, index);
        }

        private static bool IsValidIdentifierStartCharacter(char character)
        {
            return char.IsLetter(character) || character == '_';
        }

        private int ScanText()
        {
            do
            {
                char c = this.input[this.positionInInput++];

                switch (c)
                {
                    case '{':
                    case '}':
                        if (this.textBuilder.Length == 0)
                        {
                            this.textBuilder.Append(c);
                            return c;
                        }

                        this.positionInInput--;
                        return Token.Text;

                    case '\\':
                        if (this.positionInInput == this.input.Length)
                        {
                            this.errorLogger.LogError(
                                new Location(this.positionInInput, this.positionInInput), "Unexpected end of input.");
                        }
                        else if (!Utilities.MustBeEscaped(c = this.input[this.positionInInput++]))
                        {
                            this.errorLogger.LogError(
                                new Location(this.positionInInput - 2, this.positionInInput),
                                Utilities.InvariantFormat("\"{0}\" cannot be escaped.", c));
                        }

                        break;
                }

                this.textBuilder.Append(c);
            }
            while (this.positionInInput < this.input.Length);

            return Token.Text;
        }

        private int ScanTokens()
        {
            char c = this.input[this.positionInInput++];

            this.textBuilder.Append(c);

            switch (c)
            {
                case '<':
                    return this.Select('=', Token.LessOrEqual, '<');

                case '>':
                    return this.Select('=', Token.GreaterOrEqual, '>');

                default:
                    if (char.IsDigit(c))
                    {
                        this.ScanWhile(char.IsDigit);
                        return Token.Integer;
                    }

                    if (IsValidIdentifierStartCharacter(c))
                    {
                        this.ScanWhile(IsValidIdentifierCharacter);

                        return this.textBuilder.ToString().Equals("else", StringComparison.OrdinalIgnoreCase)
                                   ? Token.Else
                                   : Token.Identifier;
                    }

                    return c;
            }
        }

        private void ScanWhile(Func<string, int, bool> predicate)
        {
            this.SkipWhile(predicate);

            this.textBuilder.Append(this.input, this.tokenStart + 1, this.positionInInput - this.tokenStart - 1);
        }

        private int Select(char following, int ifFollows, int ifDoesNotFollow)
        {
            if (this.positionInInput < this.input.Length && this.input[this.positionInInput] == following)
            {
                this.textBuilder.Append(following);
                this.positionInInput++;
                return ifFollows;
            }

            return ifDoesNotFollow;
        }

        private void SkipWhile(Func<string, int, bool> predicate)
        {
            while (this.positionInInput < this.input.Length && predicate(this.input, this.positionInInput))
            {
                this.positionInInput++;
            }
        }

        #endregion
    }
}