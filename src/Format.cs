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

	using Krabicezpapundeklu.Formatting.Ast;
	using Krabicezpapundeklu.Formatting.Errors;

	public class Format
	{
		#region Constants and Fields

		private readonly AstNode ast;

		#endregion

		#region Constructors and Destructors

		private Format(AstNode ast)
		{
			this.ast = ast;
		}

		#endregion

		#region Public Methods

		public static string Evaluate(string format, params object[] arguments)
		{
			return Evaluate(format, new ArgumentCollection(arguments));
		}

		public static string Evaluate(string format, ArgumentCollection arguments)
		{
			return Evaluate(null, format, arguments);
		}

		public static string Evaluate(IFormatProvider formatProvider, string format, params object[] arguments)
		{
			return Evaluate(formatProvider, format, new ArgumentCollection(arguments));
		}

		public static string Evaluate(IFormatProvider formatProvider, string format, ArgumentCollection arguments)
		{
			return Parse(format).Evaluate(formatProvider, arguments);
		}

		public static Format Parse(string format)
		{
			return
				new Format(
					new Parser(
						new Scanner(Utilities.ThrowIfNull(format, "format"), SimpleErrorLogger.Instance),
						SimpleErrorLogger.Instance).Parse());
		}

		public string Evaluate(params object[] arguments)
		{
			return Evaluate(new ArgumentCollection(arguments));
		}

		public string Evaluate(ArgumentCollection arguments)
		{
			return Evaluate((IFormatProvider) null, arguments);
		}

		public string Evaluate(IFormatProvider formatProvider, params object[] arguments)
		{
			return Evaluate(formatProvider, new ArgumentCollection(arguments));
		}

		public string Evaluate(IFormatProvider formatProvider, ArgumentCollection arguments)
		{
			return
				new Interpreter(
					formatProvider, Utilities.ThrowIfNull(arguments, "arguments"), SimpleErrorLogger.Instance).Evaluate(
						ast);
		}

		#endregion
	}
}
