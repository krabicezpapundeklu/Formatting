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

namespace Krabicezpapundeklu.Formatting.Ast
{
	using System;

	using T = Token;

	public class Operator : AstNode
	{
		#region Constructors and Destructors

		public Operator(Location location, int token)
			: this(location, token, T.ToString(token)) {}

		public Operator(Location location, int token, string text)
			: base(location)
		{
			Token = token;
			Text = Utilities.ThrowIfNull(text, "text");

			if(!IsOperator(token))
				throw new ArgumentException(Utilities.InvariantFormat("\"{0}\" is not valid operator.", Text), "token");
		}

		#endregion

		#region Public Properties

		public bool IsBinary
		{
			get { return IsBinaryOperator(Token); }
		}

		public bool IsUnary
		{
			get { return IsUnaryOperator(Token); }
		}

		public string Text { get; private set; }

		public int Token { get; private set; }

		#endregion

		#region Public Methods

		public static bool IsBinaryOperator(int token)
		{
			switch(token)
			{
			case '=':
			case '!':
			case '>':
			case '<':
			case ',':
			case T.LessOrEqual:
			case T.GreaterOrEqual:
			case T.And:
				return true;

			default:
				return false;
			}
		}

		public static bool IsOperator(int token)
		{
			return IsBinaryOperator(token) || IsUnaryOperator(token);
		}

		public static bool IsUnaryOperator(int token)
		{
			return token == '-';
		}

		public override string ToString()
		{
			return Text;
		}

		#endregion

		#region Methods

		protected override object DoAccept(IAstVisitor visitor)
		{
			return visitor.Visit(this);
		}

		protected override AstNode DoClone(Location newLocation)
		{
			return new Operator(newLocation, Token, Text);
		}

		#endregion
	}
}
