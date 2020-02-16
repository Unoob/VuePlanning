<template>
  <v-content>
    <v-toolbar color="primary" dark>
      <v-toolbar-title> {{ $t("toolbarTitle") }}: {{ user.roomId }} </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn v-if="IsConnected" icon :color="HeartColor">
        <v-icon>fas fa-heart</v-icon>
      </v-btn>
      <v-btn v-else icon @click="start">
        <v-icon>fas fa-sync</v-icon>
      </v-btn>
      <v-btn icon @click="SignOut"><v-icon>fas fa-sign-out-alt</v-icon></v-btn>
    </v-toolbar>
    <router-view></router-view>
  </v-content>
</template>
<script>
import { HubConnectionState } from "@microsoft/signalr";
import { onFocus, start } from "@/services/HubService";
import { mapActions, mapState } from "vuex";
export default {
  name: "MasterView",
  beforeRouteEnter: function(to, from, next) {
    console.log("mv before route enter");
    start().then(() => next());
  },
  mounted: function() {
    console.log("mv mounted");
    this.$nextTick(() => {
      window.addEventListener("focus", onFocus);
    });
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
    IsConnected() {
      return this.connectionState === HubConnectionState.Connected;
    }
  },
  methods: {
    ...mapActions(["LeaveRoom"]),
    SignOut() {
      this.LeaveRoom();
      this.$router.push({ name: "login" });
    }
  },
  destroyed: function() {
    window.removeEventListener("focus", onFocus);
  }
};
</script>
