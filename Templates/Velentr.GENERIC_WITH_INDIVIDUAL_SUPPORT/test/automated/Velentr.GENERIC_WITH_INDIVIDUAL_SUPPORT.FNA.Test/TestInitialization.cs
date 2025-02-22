namespace Velentr.GENERIC_WITH_INDIVIDUAL_SUPPORT.Test;

[SetUpFixture]
public class TestInitialization
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        FnaDependencyHelper.HandleDependencies();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
