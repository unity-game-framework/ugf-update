using UGF.RuntimeTools.Runtime.Providers;

namespace UGF.Update.Runtime
{
    public interface IUpdateProvider : IProvider<IUpdateGroup, UpdateGroupFunction>
    {
        IUpdateLoop UpdateLoop { get; }
    }
}
