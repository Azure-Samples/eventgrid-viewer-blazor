using Blazor.EventGridViewer.Core.CustomEventArgs;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using System;

namespace Blazor.EventGridViewer.Services
{
    /// <summary>
    /// Class used to handle EventGrid Events
    /// </summary>
    public class EventGridService : IEventGridService
    {
        /// <inheritdoc/>
        public event EventHandler<EventGridEventArgs> EventReceived;
        private readonly IAdapter<EventGridEventModel, EventGridViewerEventModel> _eventGridEventModelAdapter;

        public EventGridService(IAdapter<EventGridEventModel, EventGridViewerEventModel> eventGridEventModelAdapter)
        {
            _eventGridEventModelAdapter = eventGridEventModelAdapter;
        }

        /// <inheritdoc/>
        public bool RaiseEventReceivedEvent(EventGridEventModel model)
        {
            if (string.IsNullOrWhiteSpace(model.EventType) || string.IsNullOrWhiteSpace(model.Subject))
                return false;

            var eventGridViewerEventModel = _eventGridEventModelAdapter.Convert(model);

            EventReceived?.Invoke(this, new EventGridEventArgs(eventGridViewerEventModel));
            return true;
        }
    }
}
