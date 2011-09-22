namespace Krabicezpapundeklu.Formatting.Ast
{
    public class Integer : AstNode, IExpression
    {
        public int Value { get; private set; }

        public Integer(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
