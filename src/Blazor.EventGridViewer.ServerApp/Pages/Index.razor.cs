using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.EventGridViewer.ServerApp.Pages
{
    /// <summary>
    /// Partial Index Razor File
    /// </summary>
    public partial class Index : IDisposable
    {
        [Inject]
        private IEventGridViewerService _eventGridViewerService { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private readonly object _modelsLock = new object();
        private List<EventGridViewerEventModel> _models = new List<EventGridViewerEventModel>();
        private List<EventGridViewerEventModel> Models
        {
            get
            {
                lock (_modelsLock)
                {
                    return new List<EventGridViewerEventModel>(_models);
                }
            }
        }

        /// <summary>
        /// Implement Dispose
        /// </summary>
        public void Dispose()
        {
            if (_eventGridViewerService != null)
            {
                _eventGridViewerService.RemoveAllRequested -= RemoveAllRequestedHandler;
                _eventGridViewerService.EventReceived -= EventReceivedHandler;
            }
        }

        /// <summary>
        /// Remove All from List
        /// </summary>
        private void RemoveAll()
        {
            _eventGridViewerService?.RaiseRemoveAllRequestedEvent();
        }

        /// <summary>
        /// Copy text to clipboard
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task onCopyClick(string data)
        {
            await _jsRuntime.InvokeVoidAsync("clipboardCopy.copyText", data);
        }

        /// <summary>
        /// After First Render
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // https://blog.ladeak.net/posts/blazor-code-highlight
            await _jsRuntime.InvokeVoidAsync("Prism.highlightAll");
        }

        /// <summary>
        /// OnInitialize Event Handler
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (_eventGridViewerService != null)
            {
                // Initialize local copy with current models (with null check)
                lock (_modelsLock)
                {
                    if (_eventGridViewerService.Models != null)
                    {
                        _models.AddRange(_eventGridViewerService.Models);
                    }
                }

                _eventGridViewerService.RemoveAllRequested += RemoveAllRequestedHandler;
                _eventGridViewerService.EventReceived += EventReceivedHandler;
            }
        }

        /// <summary>
        /// Invoke UI state change for EventReceived Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventReceivedHandler(object sender, EventArgs e)
        {
            // Update local collection safely with thread-safe operations
            lock (_modelsLock)
            {
                if (_eventGridViewerService?.Models != null)
                {
                    _models.Clear();
                    _models.AddRange(_eventGridViewerService.Models);
                }
            }
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Invoke UI state change for RemoveAllRequest Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveAllRequestedHandler(object sender, EventArgs e)
        {
            // Clear local collection safely with thread synchronization
            lock (_modelsLock)
            {
                _models.Clear();
            }
            InvokeAsync(StateHasChanged);
        }
    }
}

