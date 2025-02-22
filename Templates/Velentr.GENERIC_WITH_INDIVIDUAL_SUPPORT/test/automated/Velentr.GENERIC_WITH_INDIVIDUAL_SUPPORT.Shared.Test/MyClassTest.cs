namespace Velentr.GENERIC_WITH_INDIVIDUAL_SUPPORT.Test;

public class MyClassTest
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MyClass();
        var result = myClass.MyMethod("World");
        Assert.That(result, Is.EqualTo("Hello, World!"));
    }
}
