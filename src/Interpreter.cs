/*
Copyright 2011 krabicezpapundeklu. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY KRABICEZPAPUNDEKLU ''AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL KRABICEZPAPUNDEKLU OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of krabicezpapundeklu.
*/

namespace Krabicezpapundeklu.Formatting
{
	using System;
	using System.Text;

	using Krabicezpapundeklu.Formatting.Ast;
	using Krabicezpapundeklu.Formatting.Errors;

	using Microsoft.CSharp.RuntimeBinder;

	public class Interpreter : AstVisitor
	{
		#region Constants and Fields

		private static readonly object Error = new object();

		private readonly ArgumentCollection arguments;

		private readonly IErrorLogger errorLogger;

		private readonly IFormatProvider formatProvider;

		private readonly StringBuilder formatted = new StringBuilder();

		#endregion

		#region Constructors and Destructors

		public Interpreter(IFormatProvider formatProvider, ArgumentCollection arguments, IErrorLogger errorLogger)
		{
			this.formatProvider = formatProvider;
			this.arguments = Utilities.ThrowIfNull(arguments, "arguments");
			this.errorLogger = Utilities.ThrowIfNull(errorLogger, "errorLogger");
		}

		#endregion

		#region Public Methods

		public string Evaluate(AstNode ast)
		{
			return Visit<string>(ast);
		}

		#endregion

		#region Methods

		protected override object Default(AstNode node)
		{
			return null;
		}

		protected override object DoVisit(ArgumentIndex argumentIndex)
		{
			if(argumentIndex.Index >= arguments.Count)
			{
				errorLogger.LogError(
					argumentIndex.Location, Utilities.InvariantFormat("Argument index {0} is out of range.", argumentIndex.Index));

				return Error;
			}

			return arguments[argumentIndex.Index].Value;
		}

		protected override object DoVisit(ArgumentName argumentName)
		{
			if(!arguments.Contains(argumentName.Name))
			{
				errorLogger.LogError(
					argumentName.Location, Utilities.InvariantFormat("Argument with name \"{0}\" doesn't exist.", argumentName.Name));

				return Error;
			}

			return arguments[argumentName.Name].Value;
		}

		protected override object DoVisit(BinaryExpression binaryExpression)
		{
			dynamic leftOperand = Visit(binaryExpression.LeftExpression);
			dynamic rightOperand = Visit(binaryExpression.RightExpression);

			if((object) leftOperand == Error || (object) rightOperand == Error)
				return Error;

			try
			{
				switch(binaryExpression.Operator.Token)
				{
				case '=':
					return leftOperand == rightOperand;

				case '!':
					return leftOperand != rightOperand;

				case '>':
					return leftOperand > rightOperand;

				case '<':
					return leftOperand < rightOperand;

				case ',':
					return leftOperand || rightOperand;

				case Token.LessOrEqual:
					return leftOperand <= rightOperand;

				case Token.GreaterOrEqual:
					return leftOperand >= rightOperand;

				case Token.And:
					return leftOperand && rightOperand;

				default:
					errorLogger.LogError(
						binaryExpression.Operator.Location,
						Utilities.InvariantFormat("Invalid operator \"{0}\".", binaryExpression.Operator.Text));

					return Error;
				}
			}
			catch(RuntimeBinderException)
			{
				errorLogger.LogError(
					binaryExpression.Operator.Location,
					string.Format(
						"Operator \"{0}\" cannot be applied to operands of type \"{1}\" and \"{2}\".",
						binaryExpression.Operator.Text,
						leftOperand.GetType(),
						rightOperand.GetType()));

				return Error;
			}
		}

		protected override object DoVisit(ConditionalFormat conditionalFormat)
		{
			// to catch errors, ignore result
			Visit(conditionalFormat.Argument);

			foreach(Case @case in conditionalFormat.Cases)
			{
				object conditionResult = Visit(@case.Condition);

				if(conditionResult == Error)
				{
					// visit to catch errors, but ignore result
					Visit(@case.FormatString);
					continue;
				}

				if((bool) conditionResult)
					return Visit(@case.FormatString);
			}

			return string.Empty;
		}

		protected override object DoVisit(ConstantExpression constantExpression)
		{
			return constantExpression.Value;
		}

		protected override object DoVisit(FormatString formatString)
		{
			int originalLength = formatted.Length;

			foreach(FormatStringItem item in formatString.Items)
				formatted.Append(Visit(item));

			string formattedString = formatted.ToString(originalLength, formatted.Length - originalLength);

			formatted.Length = originalLength;

			return formattedString;
		}

		protected override object DoVisit(Integer integer)
		{
			return integer.Value;
		}

		protected override object DoVisit(SimpleFormat simpleFormat)
		{
			object argument = Visit(simpleFormat.Argument);
			object format = Visit(simpleFormat.FormatString);

			if(argument == Error || format == Error)
				return null;

			string formattedArgument = Format(argument, (string) format);

			int padding = simpleFormat.Width - formattedArgument.Length;

			if(padding > 0)
				if(simpleFormat.LeftAlign)
				{
					formatted.Append(formattedArgument);
					formatted.Append(' ', padding);
				}
				else
				{
					formatted.Append(' ', padding);
					formatted.Append(formattedArgument);
				}
			else
				formatted.Append(formattedArgument);

			return null;
		}

		protected override object DoVisit(Text text)
		{
			return text.Value;
		}

		protected override object DoVisit(UnaryExpression unaryExpression)
		{
			switch(unaryExpression.Operator.Token)
			{
			case '-':
				dynamic operand = Visit(unaryExpression.Operand);

				try
				{
					return -operand;
				}
				catch(RuntimeBinderException)
				{
					errorLogger.LogError(
						unaryExpression.Operator.Location,
						string.Format(
							"Operator \"-\" cannot be applied to operand of type \"{0}\".", operand.GetType()));

					return Error;
				}

			default:
				errorLogger.LogError(
					unaryExpression.Operator.Location,
					Utilities.InvariantFormat("Invalid operator \"{0}\".", unaryExpression.Operator.Text));

				return Error;
			}
		}

		private string Format(object argument, string format)
		{
			if(argument == null)
				return string.Empty;

			ICustomFormatter formatter = formatProvider == null
				? null
				: formatProvider.GetFormat(typeof(ICustomFormatter)) as
					ICustomFormatter;

			if(formatter != null)
				return formatter.Format(format, argument, formatProvider);

			var formattable = argument as IFormattable;

			if(formattable != null)
				return formattable.ToString(format, formatProvider);

			return argument.ToString();
		}

		#endregion
	}
}
