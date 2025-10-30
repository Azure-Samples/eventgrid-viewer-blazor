using System;
using System.Collections.Generic;
using Blazor.EventGridViewer.Core.CustomEventArgs;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;

namespace Blazor.EventGridViewer.Services
{
    public class EventGridViewerService : IEventGridViewerService, IDisposable
    {
        private readonly object _modelsLock = new object();
        private readonly List<EventGridViewerEventModel> _models;

        /// <inheritdoc/>
        public List<EventGridViewerEventModel> Models
        {
            get
            {
                lock (_modelsLock)
                {
                    return new List<EventGridViewerEventModel>(_models);
                }
            }
        }

        /// <inheritdoc/>
        public event EventHandler RemoveAllRequested;
        /// <inheritdoc/>
        public event EventHandler EventReceived;
        private readonly IEventGridService _eventGridService;

        public EventGridViewerService(IEventGridService eventGridService)
        {
            _eventGridService = eventGridService;
            _models = new List<EventGridViewerEventModel>();

            _eventGridService.EventReceived += EventReceivedHandler;
        }

        /// <inheritdoc/>
        public bool RaiseRemoveAllRequestedEvent()
        {
            lock (_modelsLock)
            {
                _models.Clear();
            }
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
            lock (_modelsLock)
            {
                _models.Insert(0, e.Model);
            }
            EventReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}
