namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
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
            foreach (var field in Helpers.GetFields<int>(typeof(Token)).Where(x => x.Value >= 0))
            {
                Assert.Fail("Token [{0}] has non-negative value {1}.", field.Key, field.Value);
            }
        }

        [Test]
        [MultipleAsserts]
        public void FieldValues_AreUnique()
        {
            foreach (var group in Helpers.GetFields<int>(typeof(Token)).GroupBy(x => x.Value).Where(x => x.Count() > 1))
            {
                Assert.Fail("Tokens [{0}] have same value {1}.", string.Join(", ", group.Select(x => x.Key)), group.Key);
            }
        }

        [Test]
        [Row('a')]
        [Row('1')]
        [Row('{')]
        [Row(char.MinValue)]
        [Row(char.MaxValue)]
        public void ToString_WhenTokenIsCharacter_ReturnsItAsString(char character)
        {
            Assert.That(Token.ToString(character), Is.EqualTo(character.ToString()));
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