# Overview

This document was created to record information found while this project was a proof of concept.  The notes can serve as a reference to note the technology being used, any issues that may have arrived during research & any customization to the code that Visual Studio generates when creating a Blazor Server project.

## Deployment

During development, the application was deployed using Azure DevOps.  Information about the pipelines can be found [here](infrastructure/azure-pipelines/README.md)

## BlazorStrap Changes

1. [Official Instructions](https://blazorstrap.io/)
1. Installed BlazorStrap NuGet Package
1. Modified Startup.cs to add the "AddBootstrapCss" to the DI container
1. Used [this](https://github.com/chanan/BlazorStrap/blob/master/src/ServerSideSample/Pages/_Host.cshtml) example to modify _Host.cshtml
1. Removed the project geneated reference to Bootstrap.css in _Host.cshtml
1. Created *MainLayout.razor.cs* (as opposed to putting it in MainLayout.razor) and add [this](https://github.com/chanan/BlazorStrap/blob/master/src/SampleCore/Shared/MainLayout.razor) code.
1. Remove the *wwwroot/css/bootstrap.min.css* file that was created by default when the Blazor project was created

## Fontawesome Changes

1. Download [here](https://fontawesome.com/how-to-use/on-the-web/setup/hosting-font-awesome-yourself)
1. Create a folder under *wwwroot/css/fontawesome* and place *all.min.css* in this directory
1. Reference *all.min.css* in _Host.cshtml
1. webfonts folder should go in *wwwroot/css* folder as this is where *all.min.css* will look for it

## PrismJS

1. Download the JavaScript & CSS file [here](https://prismjs.com/download.html#themes=prism&languages=json+json5)
1. Place files in a directory in wwwroot
1. Reference the files in *_Host.cshtml*
1. Read [this](https://prismjs.com/download.html#themes=prism&languages=json+json5) blog to understand Blazor and PrimJS
1. Add the following to Index.razor.cs

```csharp
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // https://blog.ladeak.net/posts/blazor-code-highlight
            await _jsRuntime.InvokeVoidAsync("Prism.highlightAll");
        }
```

## App Registration Name

Blazor.EventGridViewer.ServerApp

## Misc. Notes

1. For *Copy* to work, it must be https

## Misc. Links

1. [Update the UI from Server Example1](https://blazor-university.com/dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/)
1. [Update the UI from Server Example2](https://www.codeproject.com/Tips/5256345/Real-Time-HTML-Page-Content-Update-with-Blazor-and)
1. [ValueKind Issue](https://github.com/dotnet/runtime/issues/31408)
1. [Bootstrap with Blazor - No jQuery](https://www.reddit.com/r/Blazor/comments/ej5dv7/cant_seem_to_get_some_bootstrap_components_to/)
1. [Gitter Thread About Accordion](https://gitter.im/aspnet/Blazor?at=5b3262437d3bca737a0bce50)
1. [BlazorStrap](https://chanan.github.io/BlazorStrap/)
1. [BlazorStrap - Collapse](https://github.com/chanan/BlazorStrap/blob/master/src/BlazorStrap/Collapse.cshtml)
