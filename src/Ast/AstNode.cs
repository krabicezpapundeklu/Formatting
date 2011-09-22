namespace Krabicezpapundeklu.Formatting.Ast
{
    public class AstNode : IAstNode
    {
        public Location Location { get; set; }

        public AstNode()
        {
            Location = Location.Unknown;
        }
    }
}
