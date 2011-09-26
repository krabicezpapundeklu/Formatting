namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal class ParserTests
    {
        [Test]
        public void Constructor_WhenScannerIsNull_ThrowsException()
        {
            Assert.That(() => new Parser(null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [Row("")]
        [Row("xxx")]
        [Row(@"xxx\{yyy\}zzz")]
        [Row("xxx{0}xxx")]
        [Row("{123,-456}")]
        [Row("{123,-456:xxx}")]
        [Row("{0:{0}}")]
        [Row(@"{0:\{0\}}")]
        [Row("{0 {=1:aaa}}")]
        [Row("{0 {=1:aaa{0}bbb}}")]
        [Row(@"{0 {=1:aaa\{0\}bbb}}")]
        [Row("{0 {=1:aaa}{=2:aaa}}")]
        [Row("{0 {=-1:xxx}}")]
        [Row("{0 {=-{1}:xxx}}")]
        [Row("{0 {>1<3:xxx}}")]
        [Row("{0 {=1,=2,=3:xxx}}")]
        [Row("{0 {>1<5,=0:xxx}}")]
        [Row("{0 {>{1}<{5},={0}:xxx}}")]
        public void Parse_ParsesInputCorrectly(string input)
        {
            Assert.That(Helpers.CreateParser(input).Parse().ToString(), Is.EqualTo(input));
        }

        [Test]
        [MultipleAsserts]
        [Row("{0 {x", "Unknown operator \"x\".", 4, 5)]
        [Row("{0 {", "Unexpected end of input.", 4, 4)]
        [Row("{0 {=z", "Expected argument, but got \"z\".", 5, 6)]
        [Row("{0 {=", "Unexpected end of input.", 5, 5)]
        public void Parse_WhenHavingSyntaxError_ThrowsException(
            string input, string errorMessage, int errorStart, int errorEnd)
        {
            Helpers.RequireFormattingException(
                () => Helpers.CreateParser(input).Parse(), errorMessage, errorStart, errorEnd);
        }
    }
}
