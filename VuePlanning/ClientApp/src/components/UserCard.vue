<template>
  <v-card :loading="UserState(user.state)" height="250" :color="cardColor">
    <template v-slot:progress>
      <v-progress-linear
        background-color="warning"
        height="6"
        indeterminate
        color="success"
      ></v-progress-linear>
    </template>
    <transition name="fade" mode="out-in">
      <keep-alive>
        <UserCardBack v-if="!flip" :user="user" @remove="RemoveUser"></UserCardBack>
        <UserCardFront v-if="flip" :user="user"></UserCardFront>
      </keep-alive>
    </transition>
  </v-card>
</template>
<script>
import { mapActions } from "vuex";
import { HubConnectionState } from "@microsoft/signalr";
import UserCardBack from "./UserCardBack";
import UserCardFront from "./UserCardFront";
export default {
  name: "UserCard",
  components: { UserCardBack, UserCardFront },
  props: {
    user: { type: Object, default: () => ({}) },
    show: { type: Boolean, default: false }
  },
  computed: {
    flip() {
      return this.show && this.user.userState === 2;
    },
    cardColor() {
      if (this.flip) return "success lighten-3";
      if (this.user.userState === 1) return "disabled";
      else return "primary";
    }
  },
  methods: {
    ...mapActions(["RemoveUser"]),
    UserState(state) {
      return state == HubConnectionState.Disconnected;
    }
  }
};
</script>
