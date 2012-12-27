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
	using System.Text;

	public class SimpleFormat : Format
	{
		#region Constructors and Destructors

		public SimpleFormat(
			Location location, Expression argument, bool leftAlign, int width, FormatString formatString)
			: base(location, argument)
		{
			LeftAlign = leftAlign;
			Width = width;
			FormatString = Utilities.ThrowIfNull(formatString, "formatString");
		}

		#endregion

		#region Public Properties

		public FormatString FormatString { get; private set; }

		public bool LeftAlign { get; private set; }

		public int Width { get; private set; }

		#endregion

		#region Public Methods

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.Append(Argument);
			builder.Length--;

			if(Width > 0)
			{
				builder.Append(',');

				if(LeftAlign)
					builder.Append('-');

				builder.Append(Width);
			}

			string formatString = FormatString.ToString();

			if(formatString.Length > 0)
			{
				builder.Append(':');
				builder.Append(formatString);
			}

			builder.Append('}');

			return builder.ToString();
		}

		#endregion

		#region Methods

		protected override object DoAccept(IAstVisitor visitor)
		{
			return visitor.Visit(this);
		}

		protected override AstNode DoClone(Location newLocation)
		{
			return new SimpleFormat(newLocation, Argument, LeftAlign, Width, FormatString);
		}

		#endregion
	}
}
