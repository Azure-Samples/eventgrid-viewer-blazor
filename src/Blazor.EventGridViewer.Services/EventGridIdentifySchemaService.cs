using Blazor.EventGridViewer.Core;
using Blazor.EventGridViewer.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;

namespace Blazor.EventGridViewer.Services
{
    /// <summary>
    /// Class used to identify the EventGrid message schema
    /// </summary>
    public class EventGridIdentifySchemaService : IEventGridIdentifySchemaService
    {
        /// <inheritdoc/>
        public EventGridSchemaType Identify(string json)
        {
            if (IsCloudEvent(json))
                return EventGridSchemaType.CloudEvent;
            else
                return EventGridSchemaType.EventGrid;
        }

        /// <summary>
        /// Method determines if the schema is a CloudEvent
        /// </summary>
        /// <param name="json"></param>
        /// <returns>boolean</returns>
        private bool IsCloudEvent(string json)
        {
            // EGV-Classic
            // Cloud events are sent one at a time, while Grid events
            // are sent in an array. As a result, the JObject.Parse will 
            // fail for Grid events. 
            try
            {
                // Attempt to read one JSON object. 
                var eventData = JObject.Parse(json);

                // Check for the spec version property.
                var version = eventData["specversion"].Value<string>();
                if (!string.IsNullOrEmpty(version)) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
