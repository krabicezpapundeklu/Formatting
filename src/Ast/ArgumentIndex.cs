namespace Krabicezpapundeklu.Formatting.Ast
{
    public class ArgumentIndex : AstNode, IExpression
    {
        public int Index { get; private set; }

        public ArgumentIndex(int index)
        {
            Index = index;
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", Index);
        }
    }
}
