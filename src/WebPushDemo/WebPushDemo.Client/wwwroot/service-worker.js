

self.addEventListener('install', async event => {
    console.log('Instalando el service worker...');
    self.skipWaiting();
});


self.addEventListener('fetch', event => {

    return null;
});

self.addEventListener('push', event => {
    const payload = event.data.json();
    event.waitUntil(
        self.registration.showNotification('¡Mensaje importante!', {
            body: payload.message,
            icon: 'images/pwaicon.png',
            vibrate: [100, 50, 100],
            data: {url: payload.url}
        })
    );
});

self.addEventListener('notificationclick', event => {
    var navigateTo = event.notification.data.url;
    event.waitUntil(clients.openWindow(navigateTo));
})
