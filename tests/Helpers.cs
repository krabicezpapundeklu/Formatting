namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
