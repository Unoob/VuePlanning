<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-text-field v-model="message" clearable :label="$t('room.task')"></v-text-field>
      </v-col>
      <v-col cols="12" class="text-center">
        <v-row justify="space-around">
          <v-btn @click="Send" color="primary">{{ $t("room.send") }}</v-btn>
          <v-btn color="success">{{ $t("room.check") }}</v-btn>
        </v-row>
      </v-col>
    </v-row>
    <v-row>
      <v-col lg="3" md="4" cols="6" v-for="user in users" :key="user.connectionId">
        <v-card>
          <v-card-text class="text-center">{{ user.name }}:{{ user.vote }}</v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapState, mapActions } from "vuex";
export default {
  name: "Host",
    mounted: function () {
      console.log('host mounted')
    this.CreateRoom();
  },
  data() {
    return { message: "" };
  },
  computed: mapState({
    users: state => state.room.users
  }),
  methods: {
    ...mapActions(["CreateRoom", "SendMessage"]),
    Send() {
      this.SendMessage(this.message);
    }
  }
};
</script>
