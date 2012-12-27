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

namespace Krabicezpapundeklu.Formatting.Ast
{
	using System;

	public class BinaryExpression : Expression
	{
		#region Constructors and Destructors

		public BinaryExpression(
			Location location, Operator binaryOperator, Expression leftExpression, Expression rightExpression)
			: base(location)
		{
			Operator = Utilities.ThrowIfNull(binaryOperator, "binaryOperator");
			LeftExpression = Utilities.ThrowIfNull(leftExpression, "leftExpression");
			RightExpression = Utilities.ThrowIfNull(rightExpression, "rightExpression");

			if(!Operator.IsBinaryOperator(Operator.Token))
				throw new ArgumentException(
					Utilities.InvariantFormat("\"{0}\" is not binary operator.", Operator), "binaryOperator");
		}

		#endregion

		#region Public Properties

		public Expression LeftExpression { get; private set; }

		public Operator Operator { get; private set; }

		public Expression RightExpression { get; private set; }

		#endregion

		#region Public Methods

		public override string ToString()
		{
			switch(Operator.Token)
			{
			case ',':
				return string.Concat(LeftExpression, ',', RightExpression);

			case Token.And:
				return string.Concat(LeftExpression, RightExpression);

			default:
				return string.Concat(Operator, RightExpression);
			}
		}

		#endregion

		#region Methods

		protected override object DoAccept(IAstVisitor visitor)
		{
			return visitor.Visit(this);
		}

		protected override AstNode DoClone(Location newLocation)
		{
			return new BinaryExpression(newLocation, Operator, LeftExpression, RightExpression);
		}

		#endregion
	}
}
