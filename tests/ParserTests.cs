namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using MbUnit.Framework;

    using Krabicezpapundeklu.Formatting.Ast;

    using NHamcrest.Core;

    internal class ParserTests
    {
        #region Public Methods

        [Test]
        public void Constructor_WhenErrorLoggerIsNull_ThrowsException()
        {
            Assert.That(
                () => new Parser(Helpers.CreateTextScanner(string.Empty), null), Throws.An<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenScannerIsNull_ThrowsException()
        {
            Assert.That(() => new Parser(null, null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [Row("")]
        [Row("xxx")]
        [Row(@"xxx\{yyy\}zzz")]
        [Row("xxx{0}xxx")]
        [Row("{123,-456}")]
        [Row("{123,-456:xxx}")]
        [Row("{0:{0}}")]
        [Row("{ARG_NAME:{0}}")]
        [Row(@"{0:\{0\}}")]
        [Row("{0 {=1:aaa}}")]
        [Row("{0 {={ARG_NAME}:aaa}}")]
        [Row("{0 {=1:aaa{0}bbb}}")]
        [Row(@"{0 {=1:aaa\{0\}bbb}}")]
        [Row("{0 {=1:aaa}{=2:aaa}}")]
        [Row("{0 {=-1:xxx}}")]
        [Row("{0 {=-{1}:xxx}}")]
        [Row("{0 {>1<3:xxx}}")]
        [Row("{0 {=1,=2,=3:xxx}}")]
        [Row("{0 {>1<5,=0:xxx}}")]
        [Row("{0 {>{1}<{5},={0}:xxx}}")]
        [Row("{0 {>=1:aaa}}")]
        [Row("{0 {else:aaa}}")]
        public void Parse_ParsesInputCorrectly(string input)
        {
            Assert.That(Helpers.CreateParser(input).Parse().ToString(), Is.EqualTo(input));
        }

        [Test]
        [Row("{0:}", 2, 2)]
        [Row("{0:abc}", 3, 6)]
        public void Parse_StroresInnerFormatStringLocationCorrectly(string input, int expectedStart, int expectedEnd)
        {
            FormatString ast = Helpers.CreateParser(input).Parse();
            Location location = ((SimpleFormat)ast.Items[0]).FormatString.Location;

            Assert.That(location, Is.EqualTo(new Location(expectedStart, expectedEnd)));
        }

        [Test]
        [Row("", 0, 0)]
        [Row("abc", 0, 3)]
        public void Parse_StroresTopLevelFormatStringLocationCorrectly(string input, int expectedStart, int expectedEnd)
        {
            Assert.That(
                Helpers.CreateParser(input).Parse().Location, Is.EqualTo(new Location(expectedStart, expectedEnd)));
        }

        [Test]
        [MultipleAsserts]
        [Row("xx}xx", "Unescaped \"}\".", 2, 3)]
        [Row("{0 {x", "Unknown operator \"x\".", 4, 5)]
        [Row("{0 {", "Unexpected end of input.", 4, 4)]
        [Row("{0 {=z", "Expected argument, but got \"z\".", 5, 6)]
        [Row("{0 {=", "Unexpected end of input.", 5, 5)]
        [Row("{0", "Unexpected end of input.", 2, 2)]
        [Row("{x", "Unexpected end of input.", 2, 2)]
        [Row("{0 {-1", "Expected binary operator.", 4, 5)]
        [Row("{0 {else 123", "Unexpected \"123\".", 9, 12)]
        [Row("{0 {=1 else:", "\"else\" must be used alone.", 7, 11)]
        public void Parse_WhenHavingError_ThrowsException(
            string input, string errorMessage, int errorStart, int errorEnd)
        {
            Helpers.RequireFormattingException(
                () => Helpers.CreateParser(input).Parse(), errorMessage, errorStart, errorEnd);
        }

        #endregion
    }
}