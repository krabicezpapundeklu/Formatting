namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentIndex : AstNode
    {
        public int Index { get; private set; }

        public ArgumentIndex(int index)
        {
            Index = index;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}
