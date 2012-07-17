using MReplace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void Replace_DateTimeNow()
        {
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
