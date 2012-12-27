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

// ReSharper disable InconsistentNaming

namespace Krabicezpapundeklu.Formatting.Tests
{
	using System;
	using System.Globalization;
	using System.Linq;

	using MbUnit.Framework;

	using NHamcrest.Core;

	internal class TokenTests
	{
		#region Public Methods

		[Test]
		[MultipleAsserts]
		public void FieldValues_AreNegative()
		{
			foreach(var field in Helpers.GetFields<int>(typeof(Token)).Where(x => x.Value >= 0))
				Assert.Fail("Token [{0}] has non-negative value {1}.", field.Key, field.Value);
		}

		[Test]
		[MultipleAsserts]
		public void FieldValues_AreUnique()
		{
			foreach(var group in Helpers.GetFields<int>(typeof(Token)).GroupBy(x => x.Value).Where(x => x.Count() > 1))
				Assert.Fail("Tokens [{0}] have same value {1}.", string.Join(", ", group.Select(x => x.Key)), group.Key);
		}

		[Test]
		[Row('a')]
		[Row('1')]
		[Row('{')]
		[Row(char.MinValue)]
		[Row(char.MaxValue)]
		public void ToString_WhenTokenIsCharacter_ReturnsItAsString(char character)
		{
			Assert.That(Token.ToString(character), Is.EqualTo(character.ToString(CultureInfo.InvariantCulture)));
		}

		[Test]
		[Row(int.MinValue)]
		[Row(int.MaxValue)]
		[Row(char.MaxValue + 1)]
		public void ToString_WhenTokenIsInvalid_ThrowsException(int token)
		{
			Assert.That(() => Token.ToString(token), Throws.An<ArgumentException>());
		}

		[Test]
		[Row(Token.Invalid, "Invalid")]
		[Row(Token.EndOfInput, "EndOfInput")]
		public void ToString_WhenTokenIsKnownToken_ReturnsItsName(int token, string expectedName)
		{
			Assert.That(Token.ToString(token), Is.EqualTo(expectedName));
		}

		#endregion
	}
}

// ReSharper restore InconsistentNaming
