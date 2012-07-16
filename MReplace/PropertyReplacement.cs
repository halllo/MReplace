using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MReplace
{
    public class PropertyReplacement<T, R> : IDisposable
    {
        private static MethodBase Location(LambdaExpression locator)
        {
            if (locator.Body is MemberExpression)
                return (((MemberExpression)locator.Body).Member as PropertyInfo).GetGetMethod();
            else
                return ((((UnaryExpression)locator.Body).Operand as MemberExpression).Member as PropertyInfo).GetGetMethod();
        }

        MethodBase _method;
        object _oldPointer;

        internal PropertyReplacement(Expression<Func<R>> property)
        {
            _method = Location(property);
        }

        internal PropertyReplacement(Expression<Func<T, R>> property)
        {
            _method = Location(property);
        }

        public IDisposable With(Expression<Func<R>> property)
        {
            _oldPointer = _method.ReplaceWith(Location(property));
            return this;
        }

        public IDisposable With(Expression<Func<T, R>> property)
        {
            _oldPointer = _method.ReplaceWith(Location(property));
            return this;
        }

        void IDisposable.Dispose()
        {
            MethodReplaceLogic.ReplaceMethodBack(_method, _oldPointer);
        }
    }
}