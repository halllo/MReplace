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

		internal MethodReplacement(MethodBase method)
		{
			_method = method;
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

		public IDisposable With(MethodBase method)
		{
			_oldPointer = _method.ReplaceWith(method);
			return this;
		}

		public IDisposable With<T>(string method)
		{
			var privateMethod = typeof(T).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
			_oldPointer = _method.ReplaceWith(privateMethod);
			return this;
		}

		void IDisposable.Dispose()
		{
			MethodReplaceLogic.ReplaceMethodBack(_method, _oldPointer);
		}
	}
}