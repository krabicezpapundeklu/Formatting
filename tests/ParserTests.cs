namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using MbUnit.Framework;
    using NHamcrest.Core;

    class ParserTests
    {
        [Test]
        public void Constructor_WhenScannerIsNull_ThrowsException()
        {
            Assert.That(() => new Parser(null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [Row("", "")]
        [Row("xxx", "xxx")]
        [Row(@"xxx\{yyy\}zzz", "xxx{yyy}zzz")]
        [Row("xxx{0}xxx", "xxx{0}xxx")]
        [Row("xxx{123,-456}xxx", "xxx{123,-456}xxx")]
        [Row("xxx{123,-456:xxx}xxx", "xxx{123,-456:xxx}xxx")]
        [Row("xxx{0:{0}}xxx", "xxx{0:{0}}xxx")]
        [Row(@"xxx{0:\{0\}}xxx", "xxx{0:{0}}xxx")]
        public void Parse_ParsesInputCorrectly(string input, string expectedResult)
        {
            Assert.That(new Parser(new Scanner(input)).Parse().ToString(), Is.EqualTo(expectedResult));
        }
    }
}
