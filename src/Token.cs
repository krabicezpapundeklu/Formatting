﻿/*
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
	using System.Globalization;
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
			typeof(Token).GetFields().ToDictionary(x => (int) x.GetValue(null), x => x.Name);

		#endregion

		#region Public Methods

		public static string ToString(int token)
		{
			if(token >= char.MinValue && token <= char.MaxValue)
				return ((char) token).ToString(CultureInfo.InvariantCulture);

			string name;

			if(TokenNames.TryGetValue(token, out name))
				return name;

			throw new ArgumentException(Utilities.InvariantFormat("{0} is not valid token.", token));
		}

		#endregion
	}
}
