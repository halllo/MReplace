using System;
using System.Linq.Expressions;

namespace MReplace
{
    public static class Replace
    {
        public static PropertyReplacement Property(Expression<Func<object>> property)
        {
            return new PropertyReplacement(property);
        }

        public static PropertyReplacement Property<T>(Expression<Func<T, object>> property)
        {
            return new PropertyReplacement(property);
        }

        public static MethodReplacement Method(Expression<Action> method)
        {
            return new MethodReplacement(method);
        }

        public static MethodReplacement Method<T>(Expression<Action<T>> method)
        {
            return new MethodReplacement(method);
        }
    }
}
