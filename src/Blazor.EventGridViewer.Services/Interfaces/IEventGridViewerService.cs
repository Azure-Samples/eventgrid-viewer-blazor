using Blazor.EventGridViewer.Core.Models;
using System;
using System.Collections.Generic;

namespace Blazor.EventGridViewer.Services.Interfaces
{
    /// <summary>
    /// Interface used for interaction between UI & EventGrid
    /// </summary>
    public interface IEventGridViewerService
    {
        /// <summary>
        /// Local copy of EventGrid Events displayed on the UI
        /// </summary>
        List<EventGridViewerEventModel> Models { get; }
        /// <summary>
        /// Remove All from Models List Event
        /// </summary>
        event EventHandler RemoveAllRequested;
        /// <summary>
        /// Notifies UI that EventGrid Event has been received
        /// </summary>
        event EventHandler EventReceived;
        /// <summary>
        /// Remove All from Models List
        /// </summary>
        /// <returns>Boolean</returns>
        bool RaiseRemoveAllRequestedEvent();
    }
}
