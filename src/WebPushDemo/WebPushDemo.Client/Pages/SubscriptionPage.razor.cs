using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using WebPushDemo.Client;
using WebPushDemo.Client.Shared;
using WebPush.Blazor.Services;
using WebPushDemo.Client.Services;
using WebPush.Blazor.Models;

namespace WebPushDemo.Client.Pages;

public partial class SubscriptionPage
{
    [Inject]
    WebPushService PushNotificationService { get; set; }
    [Inject]
    PushNotificationServerService PushNotificationServerService { get; set; }

    SubscriptionInfo SubscriptionInfo;
    string Message;

    async Task GetSubscriptionAsync()
    {
        Message = string.Empty;
        SubscriptionInfo = await PushNotificationService.GetSubscriptionAsync();
        if(SubscriptionInfo == default)
        {
            Message = "No se pudo obtener la subscription";
        }
    }

    async Task SendSubscriptionAsync()
    {
        var Success = await PushNotificationServerService.SendSubscription(
            new WebPushSubscription
            {
                Endpoint = SubscriptionInfo.Endpoint,
                P256dh = SubscriptionInfo.P256dh,
                Auth = SubscriptionInfo.Auth
            }
            );
        if (Success)
            Message = "Datos enviados con éxito.";
        else
            Message = "Error al enviar los datos";
    }

    async Task RequestExampleNotificationAsync()
    {
        if (await PushNotificationServerService.RequestExampleNotification())
            Message = "Solicitud de notificación enviada.";
        else
            Message = "Error de solicitud de notificación.";
    }

}