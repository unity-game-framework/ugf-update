namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateSet : TestUpdateCollection<TestTarget>
    {
        protected override IUpdateCollection<TestTarget> CreateCollection()
        {
            return new UpdateSet<TestTarget>();
        }

        protected override TestTarget CreateTarget()
        {
            return new TestTarget();
        }
    }
}
