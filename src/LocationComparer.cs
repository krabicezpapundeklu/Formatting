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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class LocationComparer : IComparer<ILocated>, IComparer<Location>
    {
        #region Constants and Fields

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly LocationComparer Instance = new LocationComparer();

        #endregion

        #region Constructors and Destructors

        private LocationComparer()
        {
        }

        #endregion

        #region Public Methods

        public int Compare(ILocated x, ILocated y)
        {
            return Compare(Utilities.ThrowIfNull(x, "x").Location, Utilities.ThrowIfNull(y, "y").Location);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public int Compare(Location x, Location y)
        {
            Utilities.ThrowIfNull(x, "x");
            Utilities.ThrowIfNull(y, "y");

            if (x.Start < y.Start)
            {
                return -1;
            }

            if (x.Start > y.Start)
            {
                return 1;
            }

            if (x.End < y.End)
            {
                return -1;
            }

            return x.End > y.End ? 1 : 0;
        }

        #endregion
    }
}