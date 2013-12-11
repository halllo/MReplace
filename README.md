MReplace
========

Supports replacing public and private instance methods, static methods and properties without any profiler. I use Ziad Elmalki's technique for the replacing of two MethodBase objects (http://www.codeproject.com/Articles/37549/CLR-Injection-Runtime-Method-Replacer).

```csharp
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
```
