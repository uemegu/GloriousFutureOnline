self.addEventListener("notificationclick", (event) => {
    event.waitUntil(async function () {
        chatClient = await clients.openWindow('./?' + event.notification.data.FCM_MSG.notification.body);
    }());
});

importScripts('/__/firebase/6.1.0/firebase-app.js');
importScripts('/__/firebase/6.1.0/firebase-messaging.js');

firebase.initializeApp({
    'messagingSenderId': '送信者IDをここに書く'
});

const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function(payload) {
    const notificationTitle = payload.notification.title;
    const notificationOptions = {
        body: payload.notification.body
    };
    return self.registration.showNotification(notificationTitle, notificationOptions);
});

