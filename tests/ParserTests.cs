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
            Assert.That(new Parser(new Scanner(input)).Parse().ToString(), Is.EqualTo(input));
        }
    }
}
