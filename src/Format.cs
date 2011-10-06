namespace Krabicezpapundeklu.Formatting
{
    using System;

    using Ast;

    using Errors;

    public class Format
    {
        private readonly AstNode ast;

        private Format(AstNode ast)
        {
            this.ast = ast;
        }

        public string Evaluate(params object[] arguments)
        {
            return Evaluate(new ArgumentCollection(arguments));
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
                new Interpreter(formatProvider, Utilities.ThrowIfNull(arguments, "arguments"), new SimpleErrorLogger()).
                    Evaluate(ast);
        }

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
                    new Parser(new Scanner(Utilities.ThrowIfNull(format, "format"), new SimpleErrorLogger())).Parse());
        }
    }
}
