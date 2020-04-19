<template>
  <v-switch v-if="supported" v-model="confirm"></v-switch>
</template>
<script>
import { mapActions } from "vuex";
export default {
  name: "NotificationSettings",
  data: () => ({
    confirm: false,
    supported: false,
    isSubscribed: false
  }),
  watch: {
    confirm(permission) {
      if (permission && !this.isSubscribed) {
        this.askPermission()
          .then(this.subscribeUserToPush)
          .then(this.SaveSubscription)
          .then(() => (this.isSubscribed = true))
          .catch(err => {
            console.log(err);
            this.confirm = false;
          });
      } else if (this.isSubscribed) {
        this.unsubscribeUserFromPush().then(() => (this.isSubscribed = false));
      }
    }
  },
  created() {
    this.supported = "serviceWorker" in navigator && "PushManager" in window;
    if (this.supported) {
      this.registerServiceWorker();
    }
    this.getNotificationPermissionState().then(
      permission => (this.confirm = permission === "granted")
    );
  },
  methods: {
    ...mapActions(["SaveSubscription"]),
    registerServiceWorker() {
      return navigator.serviceWorker
        .register("/ServiceWorker.js")
        .then(function(registration) {
          console.log("Service worker successfully registered.");
          return registration;
        })
        .catch(function(err) {
          console.error("Unable to register service worker.", err);
        });
    },
    askPermission() {
      return new Promise(function(resolve, reject) {
        const permissionResult = Notification.requestPermission(function(
          result
        ) {
          resolve(result);
        });

        if (permissionResult) {
          permissionResult.then(resolve, reject);
        }
      }).then(function(permissionResult) {
        if (permissionResult !== "granted") {
          throw new Error("We weren't granted permission.");
        }
      });
    },
    subscribeUserToPush() {
      return this.registerServiceWorker()
        .then(registration => {
          const subscribeOptions = {
            userVisibleOnly: true,
            applicationServerKey: this.urlBase64ToUint8Array(
              "BPbHlo7Z4-5Z_rr-ZKte830jOf6R4tnAS8J0IonjF269gRCdu8cdj9fsRKD9RkxprzgNO-UTFarud7QP94cHJEM"
            )
          };
          return registration.pushManager.subscribe(subscribeOptions);
        })
        .then(function(pushSubscription) {
          console.log(
            "Received PushSubscription: ",
            JSON.stringify(pushSubscription)
          );
          return pushSubscription;
        });
    },
    unsubscribeUserFromPush() {
      return this.registerServiceWorker()
        .then(function(registration) {
          // Service worker is active so now we can subscribe the user.
          return registration.pushManager.getSubscription();
        })
        .then(function(subscription) {
          if (subscription) {
            console.log("unsubscribeUserFromPush", subscription);
            return subscription.unsubscribe();
          }
        })
        .catch(function(err) {
          console.error("Failed to subscribe the user.", err);
        });
    },
    getNotificationPermissionState() {
      if (navigator.permissions) {
        return navigator.permissions
          .query({ name: "notifications" })
          .then(result => {
            return result.state;
          });
      }

      return new Promise(resolve => {
        resolve(Notification.permission);
      });
    },
    urlBase64ToUint8Array(base64String) {
      /*eslint no-useless-escape: "off"*/
      var padding = "=".repeat((4 - (base64String.length % 4)) % 4);
      var base64 = (base64String + padding)
        .replace(/\-/g, "+")
        .replace(/_/g, "/");

      var rawData = window.atob(base64);
      var outputArray = new Uint8Array(rawData.length);

      for (var i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
      }
      return outputArray;
    }
  }
};
</script>
