<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-text-field
          v-model="message"
          hide-details
          clearable
          clear-icon="fas fa-times"
          :label="$t('room.task')"
        ></v-text-field>
      </v-col>
      <v-col cols="12" class="text-center">
        <v-row justify="space-around">
          <v-btn @click="Send" color="primary">{{ $t("room.send") }}</v-btn>
          <v-btn @click="show = !show" color="success">{{ $t("room.check") }}</v-btn>
        </v-row>
      </v-col>
    </v-row>
    <v-row>
      <v-col lg="3" md="4" cols="6" v-for="user in users" :key="user.connectionId">
        <UserCard :user="user" :show="show"></UserCard>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapState, mapActions } from "vuex";
import UserCard from "./UserCard";
export default {
  name: "Host",
  mounted: function() {
    console.log("host mounted");
    this.CreateRoom();
  },
  components: { UserCard },
  data() {
    return { message: "", show: false };
  },
  computed: mapState({
    users: state => state.room.users
  }),
  methods: {
    ...mapActions(["CreateRoom", "SendMessage"]),
    Send() {
      this.show = false;
      this.SendMessage(this.message);
    }
  }
};
</script>
