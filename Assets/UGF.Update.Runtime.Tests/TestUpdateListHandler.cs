namespace UGF.Update.Runtime.Tests
{
    public class TestUpdateListHandler : TestUpdateCollection<TestTarget>
    {
        protected override IUpdateCollection<TestTarget> CreateCollection()
        {
            return new UpdateListHandler<TestTarget>(item => item.OnUpdate());
        }

        protected override TestTarget CreateTarget()
        {
            return new TestTarget();
        }
    }
}
