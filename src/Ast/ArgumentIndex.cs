namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentIndex : Expression
    {
        #region Constructors and Destructors

        public ArgumentIndex(Location location, int index)
            : base(location)
        {
            this.Index = index;
        }

        #endregion

        #region Public Properties

        public int Index { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Utilities.InvariantFormat("{{{0}}}", this.Index);
        }

        #endregion

        #region Methods

        protected override object DoAccept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        protected override AstNode DoClone(Location newLocation)
        {
            return new ArgumentIndex(newLocation, this.Index);
        }

        #endregion
    }
}