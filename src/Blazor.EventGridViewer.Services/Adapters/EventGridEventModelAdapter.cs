using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using System;

namespace Blazor.EventGridViewer.Services.Adapters
{
    /// <summary>
    /// Class used to convert an EventGridEventModel to a EventGridViewerModel
    /// </summary>
    public class EventGridEventModelAdapter : IAdapter<EventGridEventModel, EventGridViewerEventModel>
    {
        /// <summary>
        /// Method used to convert a EventGridEventModel to a EventGridViewerEventModel
        /// </summary>
        /// <param name="t">EventGridEventModel</param>
        /// <returns>EventGridViewerEventModel</returns>
        public EventGridViewerEventModel Convert(EventGridEventModel t)
        {
            if (t == null)
                throw new ArgumentNullException("EventGridEventModel is null.");

            EventGridViewerEventModel model = new EventGridViewerEventModel()
            {
                Data = t.EventData,
                EventType = t.EventType,
                Subject = t.Subject,
                Id = t.Id,
                EventTime = t.EventTime
            };
            return model;
        }
    }
}
