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
            if(arguments == null)
                throw new ArgumentNullException("arguments");

            var errorLogger = new ErrorLogger();
            string result = new Interpreter(formatProvider, arguments, errorLogger).Evaluate(ast);

            errorLogger.ThrowOnErrors();

            return result;
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
            var errorLogger = new ErrorLogger();
            Format parsedFormat = Parse(format, errorLogger);

            errorLogger.ThrowOnErrors();

            return parsedFormat;
        }

        public static Format Parse(string format, IErrorLogger errorLogger)
        {
            if(format == null)
                throw new ArgumentNullException("format");

            if(errorLogger == null)
                throw new ArgumentNullException("errorLogger");

            return new Format(new Parser(new Scanner(format, errorLogger)).Parse());
        }
    }
}
