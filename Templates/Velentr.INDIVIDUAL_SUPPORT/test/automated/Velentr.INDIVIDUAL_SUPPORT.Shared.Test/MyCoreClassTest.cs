namespace Velentr.INDIVIDUAL_SUPPORT.Test;

public class MyCoreClassTest
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MyCoreClass();
        var result = myClass.MyMethod("World");
        Assert.That(result, Is.EqualTo("Hello, World!"));
    }
}
