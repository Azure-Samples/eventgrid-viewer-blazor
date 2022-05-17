using Azure.Messaging.EventGrid;
using Blazor.EventGridViewer.Core.CustomEventArgs;
using Blazor.EventGridViewer.Core.Models;
using System;

namespace Blazor.EventGridViewer.Services.Interfaces
{
    /// <summary>
    /// Interface used to handle EventGrid Events
    /// </summary>
    public interface IEventGridService
    {
        /// <summary>
        /// EventGrid Event Received
        /// </summary>
        event EventHandler<EventGridEventArgs> EventReceived;
        /// <summary>
        /// SendEvent to subscribers
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Boolean</returns>
        bool RaiseEventReceivedEvent(EventGridEventModel model);
    }
}
