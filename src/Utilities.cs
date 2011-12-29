/*
Copyright 2011 krabicezpapundeklu. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY KRABICEZPAPUNDEKLU ''AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL KRABICEZPAPUNDEKLU OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of krabicezpapundeklu.
*/
namespace Krabicezpapundeklu.Formatting
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;

    public static class Utilities
    {
        #region Public Methods

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public static string Escape(string text)
        {
            ThrowIfNull(text, "text");

            var builder = new StringBuilder();

            foreach (char c in text)
            {
                if (MustBeEscaped(c))
                {
                    builder.Append('\\');
                }

                builder.Append(c);
            }

            return builder.ToString();
        }

        public static string InvariantFormat(string format, params object[] arguments)
        {
            return string.Format(CultureInfo.InvariantCulture, format, arguments);
        }

        public static bool MustBeEscaped(char character)
        {
            return character == '\\' || character == '{' || character == '}';
        }

        public static T ThrowIfNull<T>(T value, string name) where T : class
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        #endregion
    }
}