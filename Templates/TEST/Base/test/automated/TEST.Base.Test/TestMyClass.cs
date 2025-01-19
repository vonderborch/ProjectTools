namespace TEST.Base.Test;

public class TestMyClass
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MyClass();
        var result = myClass.MyMethod("World");
        Assert.That(result, Is.EqualTo("Hello, World!"));
    }
}
