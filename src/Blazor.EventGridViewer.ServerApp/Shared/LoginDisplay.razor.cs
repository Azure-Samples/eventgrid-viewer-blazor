using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Blazor.EventGridViewer.ServerApp.Shared
{
    public partial class LoginDisplay
    {
        [Inject]
        private IConfiguration _configuration { get; set; }

        /// <summary>
        /// Check EnableAuth value
        /// </summary>
        /// <returns>boolean</returns>
        private bool EnableAuth()
        {
            var enableAuth = _configuration["EnableAuth"];
            if (enableAuth == "true")
                return true;
            else
                return false;
        }

    }
}
