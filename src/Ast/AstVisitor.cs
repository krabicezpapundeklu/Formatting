/*
Copyright 2011 krabicezpapundeklu. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY KRABICEZPAPUNDEKLU ''AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL KRABICEZPAPUNDEKLU OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of krabicezpapundeklu.
*/
namespace Krabicezpapundeklu.Formatting.Ast
{
    public abstract class AstVisitor : IAstVisitor
    {
        #region Public Methods

        public object Visit(ArgumentIndex argumentIndex)
        {
            return DoVisit(Utilities.ThrowIfNull(argumentIndex, "argumentIndex"));
        }

        public object Visit(ArgumentName argumentName)
        {
            return DoVisit(Utilities.ThrowIfNull(argumentName, "argumentName"));
        }

        public object Visit(BinaryExpression binaryExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(binaryExpression, "binaryExpression"));
        }

        public object Visit(Case @case)
        {
            return DoVisit(Utilities.ThrowIfNull(@case, "case"));
        }

        public object Visit(ConditionalFormat conditionalFormat)
        {
            return DoVisit(Utilities.ThrowIfNull(conditionalFormat, "conditionalFormat"));
        }

        public object Visit(ConstantExpression constantExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(constantExpression, "constantExpression"));
        }

        public object Visit(FormatString formatString)
        {
            return DoVisit(Utilities.ThrowIfNull(formatString, "formatString"));
        }

        public object Visit(Integer integer)
        {
            return DoVisit(Utilities.ThrowIfNull(integer, "integer"));
        }

        public object Visit(Operator @operator)
        {
            return DoVisit(Utilities.ThrowIfNull(@operator, "operator"));
        }

        public object Visit(SimpleFormat simpleFormat)
        {
            return DoVisit(Utilities.ThrowIfNull(simpleFormat, "simpleFormat"));
        }

        public object Visit(Text text)
        {
            return DoVisit(Utilities.ThrowIfNull(text, "text"));
        }

        public object Visit(UnaryExpression unaryExpression)
        {
            return DoVisit(Utilities.ThrowIfNull(unaryExpression, "unaryExpression"));
        }

        #endregion

        #region Methods

        protected abstract object Default(AstNode node);

        protected virtual object DoVisit(ArgumentIndex argumentIndex)
        {
            return this.Default(argumentIndex);
        }

        protected virtual object DoVisit(ArgumentName argumentName)
        {
            return this.Default(argumentName);
        }

        protected virtual object DoVisit(BinaryExpression binaryExpression)
        {
            return this.Default(binaryExpression);
        }

        protected virtual object DoVisit(Case @case)
        {
            return this.Default(@case);
        }

        protected virtual object DoVisit(ConditionalFormat conditionalFormat)
        {
            return this.Default(conditionalFormat);
        }

        protected virtual object DoVisit(ConstantExpression constantExpression)
        {
            return this.Default(constantExpression);
        }

        protected virtual object DoVisit(FormatString formatString)
        {
            return this.Default(formatString);
        }

        protected virtual object DoVisit(Integer integer)
        {
            return this.Default(integer);
        }

        protected virtual object DoVisit(Operator @operator)
        {
            return this.Default(@operator);
        }

        protected virtual object DoVisit(SimpleFormat simpleFormat)
        {
            return this.Default(simpleFormat);
        }

        protected virtual object DoVisit(Text text)
        {
            return this.Default(text);
        }

        protected virtual object DoVisit(UnaryExpression unaryExpression)
        {
            return this.Default(unaryExpression);
        }

        protected object Visit(AstNode node)
        {
            return Utilities.ThrowIfNull(node, "node").Accept(this);
        }

        protected T Visit<T>(AstNode node)
        {
            return (T)Visit(node);
        }

        #endregion
    }
}