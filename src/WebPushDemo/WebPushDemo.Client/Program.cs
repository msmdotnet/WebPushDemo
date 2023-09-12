using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebPush.Blazor.Options;
using WebPushDemo.Client;
using WebPushDemo.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddPushNotificationService(
    pushNotificationOptions =>
    builder.Configuration.GetSection(WebPushNotificationOptions.SectionKey)
    .Bind(pushNotificationOptions));

builder.Services.AddHttpClient<PushNotificationServerService>(client =>
client.BaseAddress = new Uri(builder.Configuration["PushNotificationServer"]));


await builder.Build().RunAsync();
