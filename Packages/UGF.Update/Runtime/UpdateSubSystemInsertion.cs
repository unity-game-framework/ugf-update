namespace UGF.Update.Runtime
{
    /// <summary>
    /// Defines insertion mode of a system into the player loop system.
    /// </summary>
    public enum UpdateSubSystemInsertion
    {
        /// <summary>
        /// Insert system before found target.
        /// </summary>
        Before = 0,

        /// <summary>
        /// Insert system after found target.
        /// </summary>
        After = 1,

        /// <summary>
        /// Insert system into found target at the top of it's subsystems.
        /// </summary>
        InsideTop = 2,

        /// <summary>
        /// Insert system into found target at the bottom of it's subsystems.
        /// </summary>
        InsideBottom = 3
    }
}
