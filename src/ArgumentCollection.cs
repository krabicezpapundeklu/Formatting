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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ArgumentCollection : Collection<Argument>
    {
        #region Constructors and Destructors

        public ArgumentCollection()
        {
        }

        public ArgumentCollection(IEnumerable<object> arguments)
        {
            foreach (object argument in Utilities.ThrowIfNull(arguments, "arguments"))
            {
                this.Add(argument);
            }
        }

        public ArgumentCollection(IEnumerable<Argument> arguments)
        {
            foreach (Argument argument in Utilities.ThrowIfNull(arguments, "arguments"))
            {
                this.Add(argument);
            }
        }

        #endregion

        #region Public Indexers

        public Argument this[string name]
        {
            get
            {
                return this.Items.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        #endregion

        #region Public Methods

        public void Add(object value)
        {
            base.Add(new Argument(value));
        }

        public void Add(string name, object value)
        {
            base.Add(new Argument(Utilities.ThrowIfNull(name, "name"), value));
        }

        public bool Contains(string name)
        {
            return this[Utilities.ThrowIfNull(name, "name")] != null;
        }

        public bool Remove(string name)
        {
            return Remove(this[Utilities.ThrowIfNull(name, "name")]);
        }

        #endregion

        #region Methods

        protected override void InsertItem(int index, Argument item)
        {
            this.Validate(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Argument item)
        {
            this.Validate(item);
            base.SetItem(index, item);
        }

        private void Validate(Argument item)
        {
            if (!string.IsNullOrEmpty(item.Name) && this.Contains(item.Name))
            {
                throw new ArgumentException(Utilities.InvariantFormat("Argument with name \"{0}\" already exists.", item.Name));
            }
        }

        #endregion
    }
}