using System;

namespace Blazor.EventGridViewer.Core.Models
{
    /// <summary>
    /// Class used as a model for EventGrid Events
    /// </summary>
    public class EventGridEventModel
    {
        /// <summary>
        /// EventGrid Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// EventGrid Event Type
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// The entire EventGrid Event
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// EventGrid Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// EventGrid Data
        /// </summary>
        public BinaryData EventData { get; set; }
        /// <summary>
        /// EventGrid Event Time
        /// </summary>
        public DateTimeOffset EventTime { get; set; }
        /// <summary>
        /// Data Version
        /// </summary>
        public string DataVersion { get; set; }
    }
}
