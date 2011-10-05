namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal static class Helpers
    {
        public static Scanner CreateTextScanner(string input)
        {
            return new Scanner(input) {State = ScannerState.ScanningText};
        }

        public static Scanner CreateTokenScanner(string input)
        {
            return new Scanner(input) {State = ScannerState.ScanningTokens};
        }

        public static Parser CreateParser(string input)
        {
            return new Parser(CreateTextScanner(input));
        }

        public static List<KeyValuePair<string, T>> GetFields<T>(Type type)
        {
            return type.GetFields().Select(x => new KeyValuePair<string, T>(x.Name, (T)x.GetValue(null))).ToList();
        }

        public static void RequireFormattingException(Action action, string errorMessage, int errorStart, int errorEnd)
        {
            var exception = Assert.Throws<FormattingException>(() => action());

            Assert.That(exception.Errors.Count, Is.EqualTo(1));
            Assert.That(exception.Errors[0].Description, Is.EqualTo(errorMessage), "Error message doesn't match.");
            Assert.That(exception.Errors[0].Location.Start, Is.EqualTo(errorStart), "Error start doesn't match.");
            Assert.That(exception.Errors[0].Location.End, Is.EqualTo(errorEnd), "Error end doesn't match.");
        }

        public static List<string> Tokenize(Scanner scanner)
        {
            var tokens = new List<string>();

            while(true)
            {
                TokenInfo tokenInfo = scanner.Scan();

                tokens.Add(tokenInfo.ToString());

                if(tokenInfo.Token == Token.EndOfInput)
                    return tokens;
            }
        }
    }
}
