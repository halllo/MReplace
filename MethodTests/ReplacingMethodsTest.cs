using Microsoft.VisualStudio.TestTools.UnitTesting;
using MReplace;

namespace MethodTests
{
	[TestClass]
	public class ReplacingMethodsTest
	{
		[TestMethod]
		public void Replace_StaticMethod()
		{
			Assert.AreEqual(2, ClassUnderTest2.StaticMethod2());
			using (Replace.Method(() => ClassUnderTest2.StaticMethod2()).With(() => ClassUnderTest1.StaticMethod1()))
			{
				Assert.AreEqual(1, ClassUnderTest2.StaticMethod2());
			}
			Assert.AreEqual(2, ClassUnderTest2.StaticMethod2());
		}

		[TestMethod]
		public void Replace_InstanceMethod()
		{
			var tc = new ClassUnderTest2();
			Assert.AreEqual(2, tc.Method2());
			using (Replace.Method<ClassUnderTest2>(c => c.Method2()).With<ClassUnderTest1>(c => c.Method1()))
			{
				Assert.AreEqual(1, tc.Method2());
			}
			Assert.AreEqual(2, tc.Method2());
		}

		[TestMethod]
		public void Replace_PrivateInstanceMethod()
		{
			var tc = new ClassUnderTest2();
			Assert.AreEqual(2, tc.CallPrivateMethod2());
			using (Replace.Method<ClassUnderTest2>("PrivateMethod2").With<ClassUnderTest1>("PrivateMethod1"))
			{
				Assert.AreEqual(1, tc.CallPrivateMethod2());
			}
			Assert.AreEqual(2, tc.CallPrivateMethod2());
		}

		[TestMethod]
		public void Replace_StaticProperty()
		{
			Assert.AreEqual(2, ClassUnderTest2.StaticProperty2);
			using (Replace.Property(() => ClassUnderTest2.StaticProperty2).With(() => ClassUnderTest1.StaticProperty1))
			{
				Assert.AreEqual(1, ClassUnderTest2.StaticProperty2);
			}
			Assert.AreEqual(2, ClassUnderTest2.StaticProperty2);
		}

		[TestMethod]
		public void Replace_InstanceProperty()
		{
			var tc = new ClassUnderTest2();
			Assert.AreEqual(2, tc.Property2);
			using (Replace.Property<ClassUnderTest2>(c => c.Property2).With<ClassUnderTest1>(c => c.Property1))
			{
				Assert.AreEqual(1, tc.Property2);
			}
			Assert.AreEqual(2, tc.Property2);
		}

		[TestMethod]
		public void Replace_MethodBasePrivateInstance()
		{
			var mPrivateMethod1 = typeof(ClassUnderTest1).GetMethod("PrivateMethod1", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			var mPrivateMethod2 = typeof(ClassUnderTest2).GetMethod("PrivateMethod2", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

			var tc = new ClassUnderTest2();
			Assert.AreEqual(2, (int)mPrivateMethod2.Invoke(tc, null));
			using (Replace.Method(mPrivateMethod2).With(mPrivateMethod1))
			{
				Assert.AreEqual(1, (int)mPrivateMethod2.Invoke(tc, null));
			}
			Assert.AreEqual(2, (int)mPrivateMethod2.Invoke(tc, null));
		}

		[TestMethod]
		public void Replace_MethodBasePublicInstance()
		{
			var mMethod1 = typeof(ClassUnderTest1).GetMethod("Method1");
			var mMethod2 = typeof(ClassUnderTest2).GetMethod("Method2");

			var tc = new ClassUnderTest2();
			Assert.AreEqual(2, (int)mMethod2.Invoke(tc, null));
			using (Replace.Method(mMethod2).With(mMethod1))
			{
				Assert.AreEqual(1, (int)mMethod2.Invoke(tc, null));
			}
			Assert.AreEqual(2, (int)mMethod2.Invoke(tc, null));
		}

		[TestMethod]
		public void Replace_DateTimeNow()
		{
			//unfortunately I cannot get this to work :(
			using (Replace.Property(() => System.DateTime.Now).With(() => FirstJanuaray2000))
			{
				Assert.AreEqual(2000, System.DateTime.Now.Year, "ngened methods dont work yet");
			}
		}

		public static System.DateTime FirstJanuaray2000
		{
			get { return new System.DateTime(2000, 1, 1); }
		}
	}
}
