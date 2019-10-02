namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateSetHandler : TestUpdateCollection<TestTarget>
    {
        protected override IUpdateCollection<TestTarget> CreateCollection()
        {
            return new UpdateSetHandler<TestTarget>(item => item.OnUpdate());
        }

        protected override TestTarget CreateTarget()
        {
            return new TestTarget();
        }
    }
}
