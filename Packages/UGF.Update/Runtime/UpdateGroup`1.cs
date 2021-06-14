namespace UGF.Update.Runtime
{
    public class UpdateGroup<TItem> : UpdateGroup, IUpdateGroup<TItem> where TItem : class
    {
        public new IUpdateCollection<TItem> Collection { get; }

        public UpdateGroup(IUpdateCollection<TItem> collection) : this(collection, new UpdateListHandler<IUpdateGroup>(item => item.Update()))
        {
        }

        public UpdateGroup(IUpdateCollection<TItem> collection, IUpdateCollection<IUpdateGroup> subGroups) : base(collection, subGroups)
        {
            Collection = collection;
        }
    }
}
