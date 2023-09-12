export const getSubscription = async (serverPublicKey) => {
    const worker = await navigator.serviceWorker.getRegistration();
    let subscription = await worker.pushManager.getSubscription();
    let isNewSubscription = false;
    if (!subscription) {
        subscription = await subscribeAsync(worker, serverPublicKey);
        isNewSubscription = true;
    }
    if (subscription) {
        return {
            endpoint: subscription.endpoint,
            p256dh: arrayBufferToBase64(subscription.getKey('p256dh')),
            auth: arrayBufferToBase64(subscription.getKey('auth')),
            isNewSubscription: isNewSubscription
        };
    }
}

async function subscribeAsync(worker, serverPublicKey) {
    try {
        return await worker.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: serverPublicKey
        });
    } catch (error) {
        if (error.name === 'NotAllowedError') {
            return null;
        }
        throw error;
    }
}

function arrayBufferToBase64(buffer) {
    let binary = '';
    let bytes = new Uint8Array(buffer);
    let len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

