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

			State = ScannerState.ScanningText;
		}

		#endregion

		#region Public Properties

		public ScannerState State { get; set; }

		#endregion

		#region Public Methods

		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static bool IsValidIdentifier(string text)
		{
			Utilities.ThrowIfNull(text, "text");

			if(text.Length == 0)
				return false;

			if(!IsValidIdentifierStartCharacter(text[0]))
				return false;

			for(int i = 1; i < text.Length; ++i)
				if(!IsValidIdentifierCharacter(text, i))
					return false;

			return true;
		}

		public TokenInfo Scan()
		{
			textBuilder.Clear();

			if(State == ScannerState.ScanningTokens)
				SkipWhile(char.IsWhiteSpace);

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
					throw new InvalidOperationException(
						Utilities.InvariantFormat("State \"{0}\" is not supported.", State));
				}
			else
				token = Token.EndOfInput;

			return new TokenInfo(
				new Location(tokenStart, positionInInput), token, textBuilder.ToString());
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
						errorLogger.LogError(
							new Location(positionInInput, positionInInput), "Unexpected end of input.");
					else if(!Utilities.MustBeEscaped(c = input[positionInInput++]))
						errorLogger.LogError(
							new Location(positionInInput - 2, positionInInput),
							Utilities.InvariantFormat("\"{0}\" cannot be escaped.", c));

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

			textBuilder.Append(c);

			switch(c)
			{
			case '<':
				return Select('=', Token.LessOrEqual, '<');

			case '>':
				return Select('=', Token.GreaterOrEqual, '>');

			default:
				if(char.IsDigit(c))
				{
					ScanWhile(char.IsDigit);
					return Token.Integer;
				}

				if(IsValidIdentifierStartCharacter(c))
				{
					ScanWhile(IsValidIdentifierCharacter);

					return textBuilder.ToString().Equals("else", StringComparison.OrdinalIgnoreCase)
						? Token.Else
						: Token.Identifier;
				}

				return c;
			}
		}

		private void ScanWhile(Func<string, int, bool> predicate)
		{
			SkipWhile(predicate);

			textBuilder.Append(input, tokenStart + 1, positionInInput - tokenStart - 1);
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

		private void SkipWhile(Func<string, int, bool> predicate)
		{
			while(positionInInput < input.Length && predicate(input, positionInInput))
				positionInInput++;
		}

		#endregion
	}
}
