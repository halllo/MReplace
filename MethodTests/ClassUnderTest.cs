﻿
namespace MethodTests
{
	public class ClassUnderTest1
	{
		public static int StaticMethod1()
		{
			return 1;
		}

		public static int StaticProperty1 { get { return 1; } }

		public int Method1()
		{
			return 1;
		}
		
		public int Property1 { get { return 1; } }

		private int PrivateMethod1()
		{
			return 1;
		}

		public int CallPrivateMethod1()
		{
			return PrivateMethod1();
		}
	}
	public class ClassUnderTest2
	{
		public static int StaticMethod2()
		{
			return 2;
		}

		public static int StaticProperty2 { get { return 2; } }

		public int Method2()
		{
			return 2;
		}

		public int Property2 { get { return 2; } }

		private int PrivateMethod2()
		{
			return 2;
		}

		public int CallPrivateMethod2()
		{
			return PrivateMethod2();
		}
	}
}
