using BlazorStrap;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.EventGridViewer.ServerApp.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IBootstrapCss BootstrapCss { get; set; }

        private bool _isServerSide = false;
        private const string BOOTSTRAP_VERSION = "4.5.0";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await BootstrapCss.SetBootstrapCss(BOOTSTRAP_VERSION);
            }
            catch (Exception)
            {
                _isServerSide = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstrun)
        {
            if (_isServerSide && firstrun)
            {
                await BootstrapCss.SetBootstrapCss(BOOTSTRAP_VERSION);
            }
        }
    }
}

