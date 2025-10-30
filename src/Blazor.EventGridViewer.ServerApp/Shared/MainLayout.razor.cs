using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.EventGridViewer.ServerApp.Shared
{
    public partial class MainLayout
    {
        protected override async Task OnInitializedAsync()
        {
            // BlazorStrap v5 handles Bootstrap CSS automatically
            await Task.CompletedTask;
        }
    }
}

