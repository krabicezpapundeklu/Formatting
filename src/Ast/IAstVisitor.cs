namespace Krabicezpapundeklu.Formatting.Ast
{
    public interface IAstVisitor
    {
        #region Public Methods

        object Visit(ArgumentIndex argumentIndex);

        object Visit(ArgumentName argumentName);

        object Visit(BinaryExpression binaryExpression);

        object Visit(Case @case);

        object Visit(ConditionalFormat conditionalFormat);

        object Visit(ConstantExpression constantExpression);

        object Visit(FormatString formatString);

        object Visit(Integer integer);

        object Visit(Operator @operator);

        object Visit(SimpleFormat simpleFormat);

        object Visit(Text text);

        object Visit(UnaryExpression unaryExpression);

        #endregion
    }
}