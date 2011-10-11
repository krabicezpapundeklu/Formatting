namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Token
    {
        #region Constants and Fields

        public const int And = -5;

        public const int Else = -8;

        public const int EndOfInput = -2;

        public const int GreaterOrEqual = -7;

        public const int Identifier = -9;

        public const int Integer = -4;

        public const int Invalid = -1;

        public const int LessOrEqual = -6;

        public const int Text = -3;

        private static readonly Dictionary<int, string> TokenNames =
            typeof(Token).GetFields().ToDictionary(x => (int)x.GetValue(null), x => x.Name);

        #endregion

        #region Public Methods

        public static string ToString(int token)
        {
            if (token >= char.MinValue && token <= char.MaxValue)
            {
                return ((char)token).ToString();
            }

            string name;

            if (TokenNames.TryGetValue(token, out name))
            {
                return name;
            }

            throw new ArgumentException(string.Format("{0} is not valid token.", token));
        }

        #endregion
    }
}