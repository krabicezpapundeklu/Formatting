namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
    using System.Globalization;

    using MbUnit.Framework;

    using NHamcrest.Core;

    internal class FormatTests
    {
        [Test]
        [Row("", new object[] {}, "")]
        [Row("xxx", new object[] {}, "xxx")]
        [Row(@"xxx\{xxx\}xxx", new object[] {}, "xxx{xxx}xxx")]
        [Row("xxx{0}xxx", new object[] {123}, "xxx123xxx")]
        [Row("{0,5}", new object[] {123}, "  123")]
        [Row("{0,-5}", new object[] {123}, "123  ")]
        [Row("{0 {=0:0}}", new object[] {123}, "")]
        [Row("{0 {=123:123}}", new object[] {123}, "123")]
        [Row("{0 {=123:x{0}x}}", new object[] {123}, "x123x")]
        [Row(@"{0 {else:x\{0\}x}}", new object[] {123}, "x{0}x")]
        [Row("{0 {=0:0}{>100<200:x}}", new object[] {123}, "x")]
        [Row("{0 {=0:0}{>100<200:x}}", new object[] {123}, "x")]
        [Row("{0 {=0,=1,>=123<200:aaa}{>100<200:bbb}}", new object[] {123}, "aaa")]
        public void Evaluate_EvaluatesFormatCorrectly(string format, object[] arguments, string expectedResult)
        {
            Assert.That(Format.Evaluate(format, arguments), Is.EqualTo(expectedResult));
        }

        [Test]
        [Row("cs", "{0:C}", new object[] {123}, "123,00 Kč")]
        [Row("en", "{0:C}", new object[] {123}, "$123.00")]
        [Row("cs", "{0,10:C}", new object[] {123}, " 123,00 Kč")]
        [Row("cs", "{0,-10:C}", new object[] {123}, "123,00 Kč ")]
        public void Evaluate_EvaluatesFormatCorrectly(
            string cultureName, string format, object[] arguments, string expectedResult)
        {
            Assert.That(Format.Evaluate(new CultureInfo(cultureName), format, arguments), Is.EqualTo(expectedResult));
        }

        [Test]
        public void Evaluate_WhenFormatIsNull_ThrowsException()
        {
            Assert.That(() => Format.Evaluate((string)null), Throws.An<ArgumentNullException>());
        }

        [Test]
        [Row("{0}", new object[]{}, "Argument index is out of range.", 1, 2)]
        [Row("{0 {={1}:}}", new object[] { 0, false }, "Operator \"=\" cannot be applied to operands of type \"System.Int32\" and \"System.Boolean\".", 4, 5)]
        [Row("{0 {=-{1}:}}", new object[] { 0, false }, "Operator \"-\" cannot be applied to operand of type \"System.Boolean\".", 5, 6)]
        public void Evaluate_WhenHavingError_ThrowsException(
            string input, object[] arguments, string errorMessage, int errorStart, int errorEnd)
        {
            Helpers.RequireFormattingException(
                () => Format.Evaluate(input, arguments), errorMessage, errorStart, errorEnd);
        }

        [Test]
        public void Parse_WhenFormatIsNull_ThrowsException()
        {
            Assert.That(() => Format.Parse(null), Throws.An<ArgumentNullException>());
        }
    }
}
