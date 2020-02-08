<template>
  <v-content>
    <v-toolbar color="primary" dark>
      <v-toolbar-title> {{ $t("toolbarTitle") }}: {{ user.roomId }} </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn icon color="pink accent-3">
        <v-icon>fas fa-heart</v-icon>
      </v-btn>
      <v-btn icon @click="SignOut"><v-icon>fas fa-sign-out-alt</v-icon></v-btn>
    </v-toolbar>
    <router-view></router-view>
  </v-content>
</template>
<script>
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
  computed: mapState(["user"]),
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
