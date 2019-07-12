namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents an object that can handler updates.
    /// </summary>
    public interface IUpdateHandler
    {
        /// <summary>
        /// Invokes on update.
        /// </summary>
        void OnUpdate();
    }
}
