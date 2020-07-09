namespace Blazor.EventGridViewer.Services.Interfaces
{
    /// <summary>
    /// Generic Interface used to handle class conversions
    /// </summary>
    /// <typeparam name="T">Original Type</typeparam>
    /// <typeparam name="U">Converted Type</typeparam>
    public interface IAdapter<T, U> where T: class
    {
        /// <summary>
        /// Generic method used to handle class conversions
        /// </summary>
        /// <param name="t">Original Type</param>
        /// <returns>Converted Type</returns>
        U Convert(T t);
    }
}
