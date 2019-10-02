# Summary
How to enable Application Insights for an ASP.NET Core 3.0 MVC webapplication.

## Enable Application Insights server-side telemetry (no Visual Studio)

Install the Application Insights SDK NuGet package for ASP.NET Core. Recommend that you always use the latest stable version.

``` xml
<ItemGroup>
	  <PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.8.0" />
</ItemGroup>
```
## Add App Insights Service to Web App

Add **services.AddApplicationInsightsTelemetry();** to the **ConfigureServices()** method in your Startup class, as in this example:

``` c#
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    // The following line enables Application Insights telemetry collection.
    services.AddApplicationInsightsTelemetry();

    // This code adds other services for your application.
    services.AddMvc();
}
```

## Setup the Instrumentation Key

Specify an instrumentation key in **appsettings.json**. 

``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ApplicationInsights": {
    "InstrumentationKey": "[Enter your instrumentation key here]"
  }
}
```

## Run your application
Run your application and make requests to it. Telemetry should now flow to Application Insights. The Application Insights SDK automatically collects incoming web requests to your application, along with the following telemetry as well.

### Live Metrics
Live Metrics can be used to quickly verify if Application Insights monitoring is configured correctly. While it might take a few minutes before telemetry starts appearing in the portal and analytics, Live Metrics would show CPU usage of the running process in near real-time. It can also show other telemetry like Requests, Dependencies, Traces, etc.

### ILogger logs
Logs emitted via ILogger of severity Warning or greater are automatically captured. Follow ILogger docs to customize which log levels are captured by Application Insights.

### Dependencies
Dependency collection is enabled by default. This article explains the dependencies that are automatically collected, and also contain steps to do manual tracking.

### Performance counters
Support for performance counters in ASP.NET Core is limited:

- SDK Versions 2.8.0 and later support cpu/memory counter in Linux. No other counter is supported in Linux. The recommended way to get system counters in Linux (and other non-Windows environments) is by using EventCounters

### EventCounter
EventCounterCollectionModule is enabled by default, and it will collect a default set of counters from .NET Core 3.0 apps. The EventCounter tutorial lists the default set of counters collected. It also has instructions on customizing the list.

## Enable Client-side Telemetry for Web Applications

In **_ViewImports.cshtml**, add injection:

```
@inject Microsoft.ApplicationInsights.AspNetCore.JavaScriptSnippet JavaScriptSnippet
```

In **_Layout.cshtml**, insert HtmlHelper at the end of the <head> section but before any other script. If you want to report any custom JavaScript telemetry from the page, inject it after this snippet:

``` html
<head>
...
	@Html.Raw(JavaScriptSnippet.FullScript)
</head>
```

**NOTE**: If your project doesn't include _Layout.cshtml, you can still add client-side monitoring. You can do this by adding the JavaScript snippet to an equivalent file that controls the <head> of all pages within your app.

## Configure the Application Insights SDK

For more information on how to further configure the Application Insights SDK, see [here](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#configure-the-application-insights-sdk).

## How can I track telemetry that's not automatically collected?

Get an instance of **TelemetryClient** by using constructor injection, and call the required TrackXXX() method on it. We don't recommend creating new TelemetryClient instances in an ASP.NET Core application. A singleton instance of TelemetryClient is already registered in the DependencyInjection container, which shares TelemetryConfiguration with rest of the telemetry.

The following example shows how to track additional telemetry from a controller.

``` c#
using Microsoft.ApplicationInsights;

public class HomeController : Controller
{
    private TelemetryClient telemetry;

    // Use constructor injection to get a TelemetryClient instance.
    public HomeController(TelemetryClient telemetry)
    {
        this.telemetry = telemetry;
    }

    public IActionResult Index()
    {
        // Call the required TrackXXX method.
        this.telemetry.TrackEvent("HomePageRequested");
        return View();
    }
```

**NOTE**: For more information about custom data reporting in Application Insights, see [Application Insights custom metrics API reference](https://docs.microsoft.com/azure/azure-monitor/app/api-custom-events-metrics/).

## Videos

- Check out this external step-by-step video to [configure Application Insights with .NET Core and Visual Studio from scratch](https://www.youtube.com/watch?v=NoS9UhcR4gA&t).

- Check out this external step-by-step video to [configure Application Insights with .NET Core and Visual Studio Code from scratch](https://youtu.be/ygGt84GDync).

## Next steps

[Explore user flows](https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-flows) to understand how users navigate through your app.

[Configure a snapshot collection](https://docs.microsoft.com/azure/application-insights/app-insights-snapshot-debugger) to see the state of source code and variables at the moment an exception is thrown.

[Use the API](https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics) to send your own events and metrics for a detailed view of your app's performance and usage.

[Use availability tests](https://docs.microsoft.com/en-us/azure/azure-monitor/app/monitor-web-app-availability) to check your app constantly from around the world.

[Dependency Injection in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection)



