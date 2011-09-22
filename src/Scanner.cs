namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Text;

    using T = Token;

    public class Scanner : TokenInfo
    {
        private readonly string input;
        private readonly StringBuilder textBuilder = new StringBuilder();

        public ScannerState State { get; set; }

        public Scanner(string input)
        {
            if(input == null)
            {
                throw new ArgumentNullException("input");
            }

            this.input = input;

            State = ScannerState.ScanningText;
            Token = T.Invalid;
        }

        public void Scan()
        {
            textBuilder.Clear();

            if(State == ScannerState.ScanningTokens)
            {
                SkipWhiteSpace();
            }

            if ((Start = End) < input.Length)
            {
                switch (State)
                {
                    case ScannerState.ScanningText:
                        ScanText();
                        break;

                    case ScannerState.ScanningTokens:
                        ScanTokens();
                        break;

                    default:
                        throw new InvalidOperationException(string.Format("State \"{0}\" is not supported.", State));
                }
            }
            else
            {
                Token = T.EndOfInput;
            }

            Text = textBuilder.ToString();
        }

        private static bool CanBeEscaped(char c)
        {
            return c == '\\' || c == '{' || c == '}';
        }

        private void ScanText()
        {
            do
            {
                char c = input[End++];

                switch(c)
                {
                    case '{':
                    case '}':
                        if (textBuilder.Length == 0)
                        {
                            textBuilder.Append(c);
                            Token = c;
                        }
                        else
                        {
                            End--;
                            Token = T.Text;
                        }

                        return;

                    case '\\':
                        if(End == input.Length || !CanBeEscaped(c = input[End++]))
                        {
                            throw new FormatException("invalid escape"); // TODO
                        }

                        break;
                }

                textBuilder.Append(c);
            }
            while (End < input.Length);

            Token = T.Text;
        }

        private void ScanTokens()
        {
            char c = input[End++];

            if(char.IsDigit(c))
            {
                while(End < input.Length && char.IsDigit(input, End))
                {
                    End++;
                }

                textBuilder.Append(input, Start, End - Start);
                Token = T.Integer;
                return;
            }

            textBuilder.Append(c);
            Token = c;
        }

        private void SkipWhiteSpace()
        {
            while(End < input.Length && char.IsWhiteSpace(input, End))
            {
                ++End;
            }
        }
    }
}
