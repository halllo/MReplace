using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MReplace
{
    public class MethodReplacement : IDisposable
    {
        private static MethodBase Location(LambdaExpression locator)
        {
            return ((MethodCallExpression)locator.Body).Method;
        }

        MethodBase _method;
        object _oldPointer;

        internal MethodReplacement(LambdaExpression method)
        {
            _method = Location(method);
        }

        public IDisposable With(Expression<Action> method)
        {
            _oldPointer = _method.ReplaceWith(Location(method));
            return this;
        }

        public IDisposable With<T>(Expression<Action<T>> method)
        {
            _oldPointer = _method.ReplaceWith(Location(method));
            return this;
        }

        void IDisposable.Dispose()
        {
            MethodReplaceLogic.ReplaceMethodBack(_method, _oldPointer);
        }
    }
}