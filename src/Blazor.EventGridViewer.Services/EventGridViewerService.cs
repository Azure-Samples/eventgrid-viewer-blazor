using Blazor.EventGridViewer.Core.CustomEventArgs;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Blazor.EventGridViewer.Services
{
    public class EventGridViewerService : IEventGridViewerService, IDisposable
    {
        /// <inheritdoc/>
        public List<EventGridViewerEventModel> Models { get; private set; }

        /// <inheritdoc/>
        public event EventHandler RemoveAllRequested;
        /// <inheritdoc/>
        public event EventHandler EventReceived;
        private readonly IEventGridService _eventGridService;

        public EventGridViewerService(IEventGridService eventGridService)
        {
            _eventGridService = eventGridService;
            Models = new List<EventGridViewerEventModel>();

            _eventGridService.EventReceived += EventReceivedHandler;
        }

        /// <inheritdoc/>
        public bool RaiseRemoveAllRequestedEvent()
        {
            Models.Clear();
            RemoveAllRequested?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Implement Dispose
        /// </summary>
        public void Dispose()
        {
            _eventGridService.EventReceived -= EventReceivedHandler;
        }

        /// <summary>
        /// Handle EventReived Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventReceivedHandler(object sender, EventGridEventArgs e)
        {
            Models.Insert(0, e.Model);
            EventReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}
