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
                throw new ArgumentException(string.Format("Argument with name \"{0}\" already exists.", item.Name));
            }
        }

        #endregion
    }
}