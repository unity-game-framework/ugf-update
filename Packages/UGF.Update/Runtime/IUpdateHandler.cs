namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents update handler with 'OnUpdate' method.
    /// </summary>
    public interface IUpdateHandler
    {
        /// <summary>
        /// Invoked on update.
        /// </summary>
        void OnUpdate();
    }
}
