using System;

namespace Blazor.EventGridViewer.Core.Models
{
    /// <summary>
    /// Class used as a model to display data onto EventGridViewer
    /// </summary>
    public class EventGridViewerEventModel
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
        /// EventGrid Data
        /// </summary>
        public BinaryData Data { get; set; }
        /// <summary>
        /// EventGrid Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// EventGrid Event Time
        /// </summary>
        public DateTimeOffset EventTime { get; set; }
        /// <summary>
        /// Unique Id for EventGridViewer Event
        /// </summary>
        public string EventGridViewerEventId { get; }
        public EventGridViewerEventModel()
        {
            EventGridViewerEventId = Guid.NewGuid().ToString("D");
        }
    }
}