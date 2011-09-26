namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal class ScannerTests
    {
        [Test]
        public void Constructor_WhenInputIsNull_ThrowsException()
        {
            Assert.That(() => new Scanner(null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [MultipleAsserts]
        [Row(@"\", 1, 1)]
        [Row(@"\x", 0, 2)]
        public void Scan_WhenScanningInvalidEscape_ThrowsException(string input, int errorStart, int errorEnd)
        {
            var error = Assert.Throws<FormattingException>(() => Helpers.CreateTextScanner(input).Scan());

            Assert.That(error.Location.Start, Is.EqualTo(errorStart), "Error start");
            Assert.That(error.Location.End, Is.EqualTo(errorEnd), "Error end");
        }

        [Test]
        [Row("", new[] {"<EndOfInput, 0, 0>"})]
        [Row("xxx", new[] {"<Text, 0, 3: xxx>", "<EndOfInput, 3, 3>"})]
        [Row("{", new[] {"<{, 0, 1: {>", "<EndOfInput, 1, 1>"})]
        [Row("}", new[] {"<}, 0, 1: }>", "<EndOfInput, 1, 1>"})]
        [Row(@"\{", new[] {"<Text, 0, 2: {>", "<EndOfInput, 2, 2>"})]
        [Row(@"\}", new[] {"<Text, 0, 2: }>", "<EndOfInput, 2, 2>"})]
        [Row(@"\\", new[] {@"<Text, 0, 2: \>", "<EndOfInput, 2, 2>"})]
        [Row("xxx{yyy}zzz",
            new[]
            {
                "<Text, 0, 3: xxx>", "<{, 3, 4: {>", "<Text, 4, 7: yyy>", "<}, 7, 8: }>", "<Text, 8, 11: zzz>",
                "<EndOfInput, 11, 11>"
            })]
        [Row(@"xxx\{yyy", new[] {"<Text, 0, 8: xxx{yyy>", "<EndOfInput, 8, 8>"})]
        [Row(@"xxx\\\{yyy", new[] {@"<Text, 0, 10: xxx\{yyy>", "<EndOfInput, 10, 10>"})]
        public void Scan_WhenScanningText_ScansItCorrectly(string input, string[] expectedTokens)
        {
            Assert.AreElementsEqual(expectedTokens, Helpers.Tokenize(Helpers.CreateTextScanner(input)));
        }

        [Test]
        [Row("", new[] {"<EndOfInput, 0, 0>"})]
        [Row("{", new[] {"<{, 0, 1: {>", "<EndOfInput, 1, 1>"})]
        [Row("}", new[] {"<}, 0, 1: }>", "<EndOfInput, 1, 1>"})]
        [Row(" + ", new[] {"<+, 1, 2: +>", "<EndOfInput, 3, 3>"})]
        [Row("123", new[] {"<Integer, 0, 3: 123>", "<EndOfInput, 3, 3>"})]
        public void Scan_WhenScanningTokens_ScansThemCorrectly(string input, string[] expectedTokens)
        {
            Assert.AreElementsEqual(expectedTokens, Helpers.Tokenize(Helpers.CreateTokenScanner(input)));
        }
    }
}
