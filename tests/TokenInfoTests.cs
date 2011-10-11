namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal class TokenInfoTests
    {
        #region Public Methods

        [Test]
        public void Constructor_WhenLocationIsNull_ThrowsException()
        {
            Assert.That(() => new TokenInfo(null, Token.Invalid, string.Empty), Throws.An<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenTextIsNull_ThrowsException()
        {
            Assert.That(() => new TokenInfo(Location.Unknown, Token.Invalid, null), Throws.An<ArgumentNullException>());
        }

        #endregion
    }
}