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
namespace Krabicezpapundeklu.Formatting.Errors
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class MultipleErrorLogger : ErrorLogger
    {
        #region Constants and Fields

        private readonly ReadOnlyCollection<Error> errors;

        private readonly List<Error> mutableErrors;

        private bool sorted;

        #endregion

        #region Constructors and Destructors

        public MultipleErrorLogger()
        {
            this.errors = new ReadOnlyCollection<Error>(this.mutableErrors = new List<Error>());
        }

        #endregion

        #region Public Properties

        public int ErrorCount
        {
            get
            {
                return this.Errors.Count;
            }
        }

        public ReadOnlyCollection<Error> Errors
        {
            get
            {
                if (!this.sorted)
                {
                    this.mutableErrors.Sort(LocationComparer.Instance);
                    this.sorted = true;
                }

                return this.errors;
            }
        }

        #endregion

        #region Methods

        protected override void DoLogError(Error error)
        {
            if (!this.mutableErrors.Any(x => x.Location.Equals(error.Location)))
            {
                this.mutableErrors.Add(error);
                this.sorted = false;
            }
        }

        #endregion
    }
}