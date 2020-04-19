function isClientFocused() {
    return clients.matchAll({
        type: 'window',
        includeUncontrolled: true
    }).then((windowClients) => {
        let clientIsFocused = false;

        for (let i = 0; i < windowClients.length; i++) {
            const windowClient = windowClients[i];
            if (windowClient.focused) {
                clientIsFocused = true;
                break;
            }
        }

        return clientIsFocused;
    });
}

self.addEventListener('install', event => {
    console.log('installing…');
});

self.addEventListener('activate', event => {

    console.log('now ready to handle!');

});

self.addEventListener('push', function (event) {
    const title = 'Treść zadania';
    const options = {
        body: event.data.text(),
        tag: 'renotify'
    };

    const promiseChain = isClientFocused()
        .then((clientIsFocused) => {
            if (clientIsFocused) {
                console.log('Don\'t need to show a notification.');
                return;

            }

            // Client isn't focused, we need to show a notification.
            return self.registration.showNotification(title, options);
        });

    event.waitUntil(promiseChain);
});

self.addEventListener('notificationclick', function (event) {
    const clickedNotification = event.notification;
    console.log('notifiaction click', event.notification)
    clickedNotification.close();
    const urlToOpen = new URL("/room", self.location.origin).href;

    const promiseChain = clients.matchAll({
        type: 'window',
        includeUncontrolled: true
    }).then((windowClients) => {
        let matchingClient = null;

        for (let i = 0; i < windowClients.length; i++) {
            const windowClient = windowClients[i];
            if (windowClient.url === urlToOpen) {
                matchingClient = windowClient;
                break;
            }
        }

        if (matchingClient) {
            return matchingClient.focus();
        } else {
            return clients.openWindow(urlToOpen);
        }
    });

    event.waitUntil(promiseChain);
});