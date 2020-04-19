<template>
  <v-card :elevation="3" :loading="loading" :disabled="loading">
    <v-card-text>
      <v-text-field
        v-model.trim="name"
        :label="$t('login.name')"
        prepend-icon="fa-user-circle"
        type="text"
      />
      <v-text-field
        v-model.trim="room"
        :label="$t('login.room')"
        :prepend-icon="roomIcon"
        type="text"
      />
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onConfirm">{{ $t("login.confirm") }}</v-btn>
      <v-btn color="primary" outlined @click="onCreate">{{ $t("login.create") }}</v-btn>
    </v-card-actions>
  </v-card>
</template>
<script>
import { mapActions } from "vuex";
export default {
  name: "LoginModal",
  data() {
    return {
      name: "",
      room: "",
      loading: false
    };
  },
  computed: {
    roomIcon() {
      return `fa-door-${this.room ? "open" : "closed"}`;
    }
  },
  methods: {
    ...mapActions(["UserUpdate"]),
    onConfirm() {
      if (!(this.name || this.room)) return;
      this.loading = true;
      this.UserUpdate({ name: this.name, roomId: this.room, isHost: false });
      this.$router.push("room");
    },
    onCreate() {
      if (!(this.name || this.room)) return;
      this.loading = true;
      this.UserUpdate({ name: this.name, roomId: this.room, isHost: true });
      this.$router.push("room");
    }
  }
};
</script>
