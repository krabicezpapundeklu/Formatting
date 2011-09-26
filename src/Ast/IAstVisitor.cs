namespace Krabicezpapundeklu.Formatting.Ast
{
    public interface IAstVisitor
    {
        void Visit(ArgumentIndex argumentIndex);
        void Visit(BinaryExpression binaryExpression);
        void Visit(Case @case);
        void Visit(ConditionalFormat conditionalFormat);
        void Visit(ConstantExpression constantExpression);
        void Visit(FormatString formatString);
        void Visit(Integer integer);
        void Visit(Operator @operator);
        void Visit(SimpleFormat simpleFormat);
        void Visit(Text text);
        void Visit(UnaryExpression unaryExpression);
    }
}
