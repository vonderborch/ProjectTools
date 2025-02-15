namespace Velentr.INDIVIDUAL_SUPPORT.Test;

public class MyClassTestFna
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MyClass();
        var result = myClass.MyMethod("FNA");
        Assert.That(result, Is.EqualTo("Hello, FNA!"));
    }
}
