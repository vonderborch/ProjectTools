namespace Velentr.DUAL_SUPPORT.Test;

public class MyClassTestMonogame
{
    [Test]
    public void TestMyMethod()
    {
        var myClass = new MySharedClass();
        var result = myClass.MyMethod("Monogame");
        Assert.That(result, Is.EqualTo("Hello, Monogame!"));
    }
}
