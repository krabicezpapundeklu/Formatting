namespace Krabicezpapundeklu.Formatting
{
    using System;

    using Krabicezpapundeklu.Formatting.Ast;
    using Krabicezpapundeklu.Formatting.Errors;

    public class Format
    {
        #region Constants and Fields

        private readonly AstNode ast;

        #endregion

        #region Constructors and Destructors

        private Format(AstNode ast)
        {
            this.ast = ast;
        }

        #endregion

        #region Public Methods

        public static string Evaluate(string format, params object[] arguments)
        {
            return Evaluate(format, new ArgumentCollection(arguments));
        }

        public static string Evaluate(string format, ArgumentCollection arguments)
        {
            return Evaluate(null, format, arguments);
        }

        public static string Evaluate(IFormatProvider formatProvider, string format, params object[] arguments)
        {
            return Evaluate(formatProvider, format, new ArgumentCollection(arguments));
        }

        public static string Evaluate(IFormatProvider formatProvider, string format, ArgumentCollection arguments)
        {
            return Parse(format).Evaluate(formatProvider, arguments);
        }

        public static Format Parse(string format)
        {
            return
                new Format(
                    new Parser(
                        new Scanner(Utilities.ThrowIfNull(format, "format"), SimpleErrorLogger.Instance),
                        SimpleErrorLogger.Instance).Parse());
        }

        public string Evaluate(params object[] arguments)
        {
            return this.Evaluate(new ArgumentCollection(arguments));
        }

        public string Evaluate(ArgumentCollection arguments)
        {
            return Evaluate((IFormatProvider)null, arguments);
        }

        public string Evaluate(IFormatProvider formatProvider, params object[] arguments)
        {
            return Evaluate(formatProvider, new ArgumentCollection(arguments));
        }

        public string Evaluate(IFormatProvider formatProvider, ArgumentCollection arguments)
        {
            return
                new Interpreter(
                    formatProvider, Utilities.ThrowIfNull(arguments, "arguments"), SimpleErrorLogger.Instance).Evaluate(
                        this.ast);
        }

        #endregion
    }
}