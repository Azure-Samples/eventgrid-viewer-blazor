using Blazor.EventGridViewer.Core;

namespace Blazor.EventGridViewer.Services.Interfaces
{
    /// <summary>
    /// Interface used to identify the EventGrid message schema
    /// </summary>
    public interface IEventGridIdentifySchemaService
    {
        /// <summary>
        /// Method used to identify the EventGrid message schema
        /// </summary>
        /// <param name="json"></param>
        /// <returns>EventGridSchemaType</returns>
        EventGridSchemaType Identify(string json);
    }
}
