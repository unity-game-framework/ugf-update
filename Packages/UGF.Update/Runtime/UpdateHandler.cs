namespace UGF.Update.Runtime
{
    /// <summary>
    /// Represents delegate to handle item of the specified type.
    /// </summary>
    /// <param name="item">The item.</param>
    public delegate void UpdateHandler<in TItem>(TItem item);
}
