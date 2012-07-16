using System;
using System.Linq.Expressions;

namespace MReplace
{
    public static class Replace
    {
        public static PropertyReplacement<object, R> Property<R>(Expression<Func<R>> property)
        {
            return new PropertyReplacement<object, R>(property);
        }

        public static PropertyReplacement<T, object> Property<T>(Expression<Func<T, object>> property)
        {
            return new PropertyReplacement<T, object>(property);
        }

        public static MethodReplacement<object> Method(Expression<Action> method)
        {
            return new MethodReplacement<object>(method);
        }

        public static MethodReplacement<T> Method<T>(Expression<Action<T>> method)
        {
            return new MethodReplacement<T>(method);
        }
    }
}
