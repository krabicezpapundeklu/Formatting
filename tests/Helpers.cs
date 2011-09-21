namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class Helpers
    {
        public static List<KeyValuePair<string, T>> GetFields<T>(Type type)
        {
            return type.GetFields().Select(x => new KeyValuePair<string, T>(x.Name, (T) x.GetValue(null))).ToList();
        }

        public static List<string> Tokenize(Scanner scanner)
        {
            var tokens = new List<string>();

            do
            {
                scanner.Scan();

                string text =
                    scanner.Text.Length > 0 ? string.Format(": {0}", scanner.Text) : string.Empty;

                tokens.Add(string.Format("<{0}, {1}, {2}{3}>",
                    Token.ToString(scanner.Token), scanner.Start, scanner.End, text));
            }
            while (scanner.Token != Token.EndOfInput);
            
            return tokens;
        }
    }
}
