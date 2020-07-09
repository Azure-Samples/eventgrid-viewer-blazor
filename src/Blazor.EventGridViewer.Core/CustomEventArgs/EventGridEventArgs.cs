using Blazor.EventGridViewer.Core.Models;
using System;

namespace Blazor.EventGridViewer.Core.CustomEventArgs
{
    /// <summary>
    /// Class used for custom EventGrid EventArgs
    /// </summary>
    public class EventGridEventArgs : EventArgs
    {
        /// <summary>
        /// Event model
        /// </summary>
        public EventGridViewerEventModel Model { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">EventGridViewerEventModel</param>
        public EventGridEventArgs(EventGridViewerEventModel model)
        {
            Model = model;
        }
    }
}
