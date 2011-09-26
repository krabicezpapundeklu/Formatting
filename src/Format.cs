namespace Krabicezpapundeklu.Formatting
{
    using System;

    using Ast;

    public class Format
    {
        private readonly AstNode parsed;

        private Format(AstNode parsed)
        {
            this.parsed = parsed;
        }

        public string Evaluate(params object[] arguments)
        {
            return Evaluate((IFormatProvider)null, arguments);
        }

        public string Evaluate(IFormatProvider formatProvider, params object[] arguments)
        {
            return (string)parsed.Accept(new Interpreter(formatProvider, arguments));
        }

        public static string Evaluate(string format, params object[] arguments)
        {
            return Evaluate(null, format, arguments);
        }

        public static string Evaluate(IFormatProvider formatProvider, string format, params object[] arguments)
        {
            return Parse(format).Evaluate(formatProvider, arguments);
        }

        public static Format Parse(string format)
        {
            if(format == null)
                throw new ArgumentNullException("format");

            return new Format(new Parser(new Scanner(format)).Parse());
        }
    }
}
