namespace Krabicezpapundeklu.Formatting.Ast
{
    public class AstNode : IAstNode
    {
        public int Start { get; set; }
        public int End { get; set; }

        public AstNode()
        {
            Start = -1;
            End = -1;
        }
    }
}
