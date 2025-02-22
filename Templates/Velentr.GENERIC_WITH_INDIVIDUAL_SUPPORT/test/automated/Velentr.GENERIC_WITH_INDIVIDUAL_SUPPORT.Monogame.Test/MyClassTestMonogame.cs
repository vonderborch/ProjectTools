namespace Velentr.GENERIC_WITH_INDIVIDUAL_SUPPORT.Test;

public class MyClassTestMonogame
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MyClass();
        var result = myClass.MyMethod("Monogame");
        Assert.That(result, Is.EqualTo("Hello, Monogame!"));
    }
}
