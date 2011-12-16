namespace Krabicezpapundeklu.Formatting
{
    public class Argument
    {
        #region Constructors and Destructors

        public Argument(object value)
            : this(string.Empty, value)
        {
        }

        public Argument(string name, object value)
        {
            this.Name = Utilities.ThrowIfNull(name, "name");
            this.Value = value;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public object Value { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Utilities.InvariantFormat("Name = {0}, Value = {1}", this.Name, this.Value);
        }

        #endregion
    }
}