﻿/*
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
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	public class Location : IEquatable<Location>
	{
		#region Constants and Fields

		[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
		public static readonly Location Unknown = new Location();

		#endregion

		#region Constructors and Destructors

		public Location(int start, int end)
		{
			if(start < 0)
				throw new ArgumentOutOfRangeException("start");

			if(end < 0)
				throw new ArgumentOutOfRangeException("end");

			Start = start;
			End = end;
		}

		private Location() {}

		#endregion

		#region Public Properties

		public int End { get; private set; }

		public bool IsKnown
		{
			// ReSharper disable PossibleUnintendedReferenceComparison
			get { return this != Unknown; }
			// ReSharper restore PossibleUnintendedReferenceComparison
		}

		public int Length
		{
			get { return End - Start; }
		}

		public int Start { get; private set; }

		#endregion

		#region Public Methods

		public static Location FromRange(params ILocated[] items)
		{
			return FromRange((IEnumerable<ILocated>) items);
		}

		public static Location FromRange(IEnumerable<ILocated> items)
		{
			// ReSharper disable PossibleMultipleEnumeration
			Utilities.ThrowIfNull(items, "items");
			// ReSharper restore PossibleMultipleEnumeration

			bool hasSomeItem = false;

			int start = int.MaxValue;
			int end = int.MinValue;

			// ReSharper disable PossibleMultipleEnumeration
			// ReSharper disable PossibleUnintendedReferenceComparison
			foreach(ILocated item in items.Where(x => x != null && x.Location != Unknown))
			{
				hasSomeItem = true;
				start = Math.Min(start, item.Location.Start);
				end = Math.Max(end, item.Location.End);
			}
			// ReSharper restore PossibleUnintendedReferenceComparison
			// ReSharper restore PossibleMultipleEnumeration

			return hasSomeItem ? new Location(start, end) : Unknown;
		}

		public bool Equals(Location other)
		{
			if(other == null)
				return false;

			if(!other.IsKnown)
				return !IsKnown;

			return other.Start == Start && other.End == End;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Location);
		}

		public override int GetHashCode()
		{
			return IsKnown ? 1 + Start ^ End : 0;
		}

		public override string ToString()
		{
			// ReSharper disable PossibleUnintendedReferenceComparison
			return this == Unknown ? "Unknown" : Utilities.InvariantFormat("{0}, {1}", Start, End);
			// ReSharper restore PossibleUnintendedReferenceComparison
		}

		#endregion
	}
}
