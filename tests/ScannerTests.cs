namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using Errors;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal class ScannerTests
    {
        [Test]
        public void Constructor_WhenInputIsNull_ThrowsException()
        {
            Assert.That(() => new Scanner(null, null), Throws.An<ArgumentNullException>());
        }

        [Test]
        public void Constructor_WhenErrorLoggerIsNull_ThrowsException()
        {
            Assert.That(() => new Scanner(string.Empty, null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [MultipleAsserts]
        [Row(@"\", "Unexpected end of input.", 1, 1)]
        [Row(@"\x", "\"x\" cannot be escaped.", 0, 2)]
        public void Scan_WhenHavingError_ThrowsException(
            string input, string errorMessage, int errorStart, int errorEnd)
        {
            var errorLogger = new ErrorLogger();
            var scanner = new Scanner(input, errorLogger);

            scanner.Scan();

            Helpers.RequireFormattingException(errorLogger.ThrowOnErrors, errorMessage, errorStart, errorEnd);
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
        [Row("<", new[] {"<<, 0, 1: <>", "<EndOfInput, 1, 1>"})]
        [Row("<=", new[] {"<LessOrEqual, 0, 2: <=>", "<EndOfInput, 2, 2>"})]
        [Row(">", new[] {"<>, 0, 1: >>", "<EndOfInput, 1, 1>"})]
        [Row(">=", new[] {"<GreaterOrEqual, 0, 2: >=>", "<EndOfInput, 2, 2>"})]
        [Row("else", new[] {"<Else, 0, 4: else>", "<EndOfInput, 4, 4>"})]
        [Row("else x", new[] {"<Else, 0, 4: else>", "<Identifier, 5, 6: x>", "<EndOfInput, 6, 6>"})]
        [Row("else$", new[] {"<Else, 0, 4: else>", "<$, 4, 5: $>", "<EndOfInput, 5, 5>"})]
        [Row("e", new[] {"<Identifier, 0, 1: e>", "<EndOfInput, 1, 1>"})]
        public void Scan_WhenScanningTokens_ScansThemCorrectly(string input, string[] expectedTokens)
        {
            Assert.AreElementsEqual(expectedTokens, Helpers.Tokenize(Helpers.CreateTokenScanner(input)));
        }
    }
}
