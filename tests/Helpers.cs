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

namespace Krabicezpapundeklu.Formatting.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Krabicezpapundeklu.Formatting.Errors;

	using MbUnit.Framework;

	using NHamcrest.Core;

	internal static class Helpers
	{
		#region Public Methods

		public static Parser CreateParser(string input)
		{
			return new Parser(CreateTextScanner(input), SimpleErrorLogger.Instance);
		}

		public static Scanner CreateTextScanner(string input)
		{
			return new Scanner(input, SimpleErrorLogger.Instance) { State = ScannerState.ScanningText };
		}

		public static Scanner CreateTokenScanner(string input)
		{
			return new Scanner(input, SimpleErrorLogger.Instance) { State = ScannerState.ScanningTokens };
		}

		public static List<KeyValuePair<string, T>> GetFields<T>(Type type)
		{
			return type.GetFields().Select(x => new KeyValuePair<string, T>(x.Name, (T) x.GetValue(null))).ToList();
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

		#endregion
	}
}
