<template>
  <v-card
    :loading="UserState(user.state)"
    height="250"
    :color="show ? 'success lighten-3' : 'primary'"
    :dark="!show"
  >
    <template v-slot:progress>
      <v-progress-linear
        background-color="warning"
        height="6"
        indeterminate
        color="success"
      ></v-progress-linear>
    </template>

    <v-scroll-x-transition>
      <UserCardBack v-if="!show" :user="user" @remove="RemoveUser"></UserCardBack>
    </v-scroll-x-transition>
    <v-scroll-x-transition>
      <UserCardFront v-if="show" :user="user"></UserCardFront>
    </v-scroll-x-transition>
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
  methods: {
    ...mapActions(["RemoveUser"]),
    UserState(state) {
      return state == HubConnectionState.Disconnected;
    }
  }
};
</script>
