using System;
using System.Linq.Expressions;
using System.Reflection;

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

		public static MethodReplacement Method(MethodBase method)
		{
			return new MethodReplacement(method);
		}

		public static MethodReplacement Method<T>(string method)
		{
			var privateMethod = typeof(T).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
			return new MethodReplacement(privateMethod);
		}
	}
}
