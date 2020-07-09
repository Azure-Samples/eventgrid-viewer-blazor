using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private List<EventGridViewerEventModel> Models => _eventGridViewerService.Models;

        /// <summary>
        /// Implement Dispose
        /// </summary>
        public void Dispose()
        {
            _eventGridViewerService.RemoveAllRequested -= RemoveAllRequestedHandler;
            _eventGridViewerService.EventReceived -= EventReceivedHandler;
        }

        /// <summary>
        /// Remove All from List
        /// </summary>
        private void RemoveAll()
        {
            _eventGridViewerService.RaiseRemoveAllRequestedEvent();
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
            _eventGridViewerService.RemoveAllRequested += RemoveAllRequestedHandler;
            _eventGridViewerService.EventReceived += EventReceivedHandler;
        }

        /// <summary>
        /// Invoke UI state change for EventReceived Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventReceivedHandler(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Invoke UI state change for RemoveAllRequest Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveAllRequestedHandler(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}

