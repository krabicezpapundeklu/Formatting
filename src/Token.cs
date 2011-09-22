namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Token
    {
        public const int Invalid = -1;

        public const int EndOfInput = -2;
        public const int Text = -3;
        public const int Integer = -4;

        public const int And = -5;

        private static readonly Dictionary<int, string> tokenNames =
            typeof (Token).GetFields().ToDictionary(x => (int) x.GetValue(null), x => x.Name);

        public static string ToString(int token)
        {
            if(token >= char.MinValue && token <= char.MaxValue)
            {
                return ((char) token).ToString();
            }

            string name;

            if(tokenNames.TryGetValue(token, out name))
            {
                return name;
            }

            throw new ArgumentException(string.Format("{0} is not valid token.", token));
        }
    }
}
