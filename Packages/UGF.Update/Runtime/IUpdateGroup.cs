namespace UGF.Update.Runtime
{
    public interface IUpdateGroup : IUpdateCollection<IUpdateGroup>
    {
        string Name { get; }
        bool Enable { get; set; }
    }
}
