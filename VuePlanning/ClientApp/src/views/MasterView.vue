<template>
  <v-content>
    <v-toolbar color="primary" dark>
      <v-toolbar-title>{{ $t("toolbarTitle") }}: {{ user.roomId }}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-title>
        <i>{{ user.name }}</i>
      </v-toolbar-title>
      <v-btn v-if="IsConnected" icon :color="HeartColor">
        <v-icon>fas fa-heart</v-icon>
      </v-btn>
      <v-btn v-else icon @click="Reconnect">
        <v-icon>fas fa-sync</v-icon>
      </v-btn>
      <v-btn v-if="!user.isHost" icon @click="SetState">
        <v-icon>far {{ UserState }}</v-icon>
      </v-btn>
      <v-btn icon @click="SignOut">
        <v-icon>fas fa-sign-out-alt</v-icon>
      </v-btn>
    </v-toolbar>
    <router-view></router-view>
  </v-content>
</template>
<script>
import { HubConnectionState } from "@microsoft/signalr";
import { onFocus, start } from "@/services/HubService";
import { mapActions, mapState } from "vuex";
const UserState = {
  AWAY: 1,
  AVAILABLE: 2
};
export default {
  name: "MasterView",
  beforeRouteEnter: function(to, from, next) {
    console.log("mv before route enter");
    start().then(
      () => next(),
      state => {
        if (state !== HubConnectionState.Disconnected) {
          next();
        }
        console.log("Current connection", state);
      }
    );
  },
  computed: {
    ...mapState(["user", "connectionState"]),
    HeartColor() {
      let color = "";
      switch (this.connectionState) {
        case HubConnectionState.Connected:
          color = "pink";
          break;
        case HubConnectionState.Reconnecting:
          color = "orange";
          break;
      }
      return `${color} accent-3`;
    },
    UserState() {
      return this.user.userState === UserState.AWAY ? "fa-play-circle" : "fa-pause-circle";
    },
    IsConnected() {
      return this.connectionState === HubConnectionState.Connected;
    }
  },
  mounted: function() {
    console.log("mv mounted");
    this.$nextTick(() => {
      window.addEventListener("focus", onFocus);
    });
  },
  destroyed: function() {
    window.removeEventListener("focus", onFocus);
  },
  methods: {
    ...mapActions(["LeaveRoom", "SetUserState"]),
    SignOut() {
      this.LeaveRoom();
      this.$router.push({ name: "login" });
    },
    Reconnect() {
      this.start();
    },
    SetState() {
      let userState = UserState.AWAY;
      if (this.user.userState === UserState.AWAY) {
        userState = UserState.AVAILABLE;
      }
      this.SetUserState(userState);
    }
  }
};
</script>
