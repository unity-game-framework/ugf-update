namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateList : TestUpdateCollection<TestTarget>
    {
        protected override IUpdateCollection<TestTarget> CreateCollection()
        {
            return new UpdateList<TestTarget>();
        }

        protected override TestTarget CreateTarget()
        {
            return new TestTarget();
        }
    }
}
