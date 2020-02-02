<template>
  <v-content>
    <v-toolbar color="primary" dark>
      <v-toolbar-title>
        {{ $t("toolbarTitle") }}
      </v-toolbar-title>
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
import { mapActions } from "vuex";
export default {
  name: "MasterView",
  mounted: function() {
    this.$nextTick(() => {
      //window.addEventListener('focus', () => alert('ny'))
    });
  },
  methods: {
    ...mapActions(["LeaveRoom"]),
    SignOut() {
      this.LeaveRoom();
      this.$router.push({ name: "login" });
    }
  }
};
</script>
