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
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;

	using Krabicezpapundeklu.Formatting.Ast;
	using Krabicezpapundeklu.Formatting.Errors;

	public class Parser
	{
		#region Constants and Fields

		private readonly IErrorLogger errorLogger;

		private readonly Scanner scanner;

		private TokenInfo currentTokenInfo;

		private TokenInfo nextTokenInfo;

		#endregion

		#region Constructors and Destructors

		public Parser(Scanner scanner, IErrorLogger errorLogger)
		{
			this.scanner = Utilities.ThrowIfNull(scanner, "scanner");
			this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");

			Consume();
		}

		#endregion

		#region Public Methods

		public FormatString Parse()
		{
			return ParseFormatString(0);
		}

		#endregion

		#region Methods

		private static FormatString CreateFormatString(int start, ICollection<FormatStringItem> items)
		{
			return items.Count == 0
				? new FormatString(new Location(start, start), items) // to have "known" location
				: new FormatString(Location.FromRange(items), items);
		}

		private static FormattingException SyntaxError(TokenInfo tokenInfo, string format, params object[] arguments)
		{
			return tokenInfo.Token == Token.EndOfInput
				? new FormattingException(tokenInfo.Location, "Unexpected end of input.")
				: new FormattingException(tokenInfo.Location, Utilities.InvariantFormat(format, arguments));
		}

		private bool Accept(int token)
		{
			if(nextTokenInfo.Token == token)
			{
				Consume();
				return true;
			}

			return false;
		}

		private void Consume()
		{
			currentTokenInfo = nextTokenInfo;
			nextTokenInfo = scanner.Scan();
		}

		private TokenInfo Expect(int token)
		{
			if(!Accept(token))
				throw SyntaxError(nextTokenInfo, "Unexpected \"{0}\".", nextTokenInfo.Text);

			return currentTokenInfo;
		}

		private Expression ParseArgumentReference()
		{
			switch(nextTokenInfo.Token)
			{
			case Token.Identifier:
				Consume();
				return new ArgumentName(currentTokenInfo.Location, currentTokenInfo.Text);

			case Token.Integer:
				Consume();
				return new ArgumentIndex(currentTokenInfo.Location, int.Parse(currentTokenInfo.Text));

			default:
				throw SyntaxError(
					nextTokenInfo, "Expected argument index or name, but got \"{0}\".", nextTokenInfo.Text);
			}
		}

		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
			MessageId =
				"Krabicezpapundeklu.Formatting.Ast.ConstantExpression.#ctor(Krabicezpapundeklu.Formatting.Location,System.Object,System.String)"
			)]
		private Expression ParseCondition(Expression implicitOperand)
		{
			if(nextTokenInfo.Token == Token.Else)
			{
				Consume();
				return new ConstantExpression(currentTokenInfo.Location, true, "else");
			}

			Expression condition = null;

			do
			{
				Expression andExpression = null;

				do
				{
					if(nextTokenInfo.Token == Token.Else)
						throw new FormattingException(nextTokenInfo.Location, "\"else\" must be used alone.");

					Operator @operator = ParseOperator();

					if(!@operator.IsBinary)
						throw new FormattingException(@operator.Location, "Expected binary operator.");

					Expression rightOperand = ParseUnaryExpression();

					var expression = new BinaryExpression(
						Location.FromRange(@operator, rightOperand), @operator, implicitOperand, rightOperand);

					andExpression = andExpression == null
						? expression
						: new BinaryExpression(
							Location.FromRange(andExpression, expression),
							new Operator(Location.Unknown, Token.And, string.Empty),
							andExpression,
							expression);
				}
				while(nextTokenInfo.Token != ':' && nextTokenInfo.Token != ',');

				Accept(',');

				condition = condition == null
					? andExpression
					: new BinaryExpression(
						Location.FromRange(condition, andExpression),
						new Operator(currentTokenInfo.Location, ','),
						condition,
						andExpression);
			}
			while(nextTokenInfo.Token != ':');

			return condition;
		}

		private ConditionalFormat ParseConditionalFormat(Expression argument)
		{
			var cases = new List<Case>();

			do
			{
				int start = Expect('{').Location.Start;
				Expression condition = ParseCondition(argument);

				scanner.State = ScannerState.ScanningText;

				Expect(':');

				FormatString formatString = ParseFormatString(nextTokenInfo.Location.Start);
				int end = Expect('}').Location.End;

				scanner.State = ScannerState.ScanningTokens;

				cases.Add(new Case(new Location(start, end), condition, formatString));
			}
			while(nextTokenInfo.Token == '{');

			return new ConditionalFormat(Location.Unknown, argument, cases);
		}

		private Ast.Format ParseFormat()
		{
			scanner.State = ScannerState.ScanningTokens;

			int start = Expect('{').Location.Start;

			Expression argument = ParseArgumentReference();

			Ast.Format format;

			if(nextTokenInfo.Token == '{')
				format = ParseConditionalFormat(argument);
			else
				format = ParseSimpleFormat(argument);

			scanner.State = ScannerState.ScanningText;

			return (Ast.Format) format.Clone(new Location(start, Expect('}').Location.End));
		}

		private FormatString ParseFormatString(int start)
		{
			var items = new List<FormatStringItem>();

			while(true)
			{
				switch(nextTokenInfo.Token)
				{
				case '{':
					try
					{
						items.Add(ParseFormat());
					}
					catch(FormattingException e)
					{
						foreach(Error error in e.Errors)
							errorLogger.LogError(error);

						scanner.State = ScannerState.ScanningText;

						while(nextTokenInfo.Token != '}' && nextTokenInfo.Token != Token.EndOfInput)
							Consume();

						break;
					}

					continue;

				case '}':
					if(start > 0)
						return CreateFormatString(start, items);

					errorLogger.LogError(nextTokenInfo.Location, "Unescaped \"}\".");
					break;

				case Token.EndOfInput:
					return CreateFormatString(start, items);

				case Token.Text:
					items.Add(new Text(nextTokenInfo.Location, nextTokenInfo.Text));
					break;

				default:
					// this should not happen
					throw new InvalidOperationException(
						Utilities.InvariantFormat("Token {0} is not valid here.", Token.ToString(nextTokenInfo.Token)));
				}

				Consume();
			}
		}

		private Operator ParseOperator()
		{
			if(Operator.IsOperator(nextTokenInfo.Token))
			{
				Consume();

				return new Operator(
					currentTokenInfo.Location, currentTokenInfo.Token, currentTokenInfo.Text);
			}

			throw SyntaxError(nextTokenInfo, "Unknown operator \"{0}\".", nextTokenInfo.Text);
		}

		private Expression ParsePrimaryExpression()
		{
			Consume();

			switch(currentTokenInfo.Token)
			{
			case '{':
				Expression argument = ParseArgumentReference();
				Expect('}');
				return argument;

			case Token.Integer:
				return new Integer(currentTokenInfo.Location, int.Parse(currentTokenInfo.Text));

			default:
				throw SyntaxError(
					currentTokenInfo, "Expected argument, but got \"{0}\".", currentTokenInfo.Text);
			}
		}

		private SimpleFormat ParseSimpleFormat(Expression argument)
		{
			bool leftAlign;
			int width;

			if(Accept(','))
			{
				leftAlign = Accept('-');
				width = int.Parse(Expect(Token.Integer).Text);
			}
			else
			{
				leftAlign = false;
				width = 0;
			}

			scanner.State = ScannerState.ScanningText;

			return new SimpleFormat(
				Location.Unknown,
				argument,
				leftAlign,
				width,
				Accept(':') ? ParseFormatString(currentTokenInfo.Location.Start) : FormatString.Empty);
		}

		private Expression ParseUnaryExpression()
		{
			if(Operator.IsUnaryOperator(nextTokenInfo.Token))
			{
				Operator @operator = ParseOperator();
				Expression expression = ParsePrimaryExpression();

				return new UnaryExpression(Location.FromRange(@operator, expression), @operator, expression);
			}

			return ParsePrimaryExpression();
		}

		#endregion
	}
}
