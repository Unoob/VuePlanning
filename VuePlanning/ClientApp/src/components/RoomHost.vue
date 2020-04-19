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
          @keyup.enter="Send"
        ></v-text-field>
      </v-col>
      <v-col cols="12" class="text-center">
        <v-row justify="space-around">
          <v-btn color="primary" @click="Send">{{ $t("room.send") }}</v-btn>
          <v-btn color="success" @click="show = !show">{{ $t("room.check") }} {{ votes }}/{{ users.length }}</v-btn>
        </v-row>
      </v-col>
    </v-row>
    <v-row>
      <v-col v-for="user in users" :key="user.connectionId" lg="3" md="4" cols="6">
        <UserCard :user="user" :show="show"></UserCard>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapState, mapActions, mapGetters } from "vuex";
import UserCard from "./UserCard";
export default {
  name: "Host",
  components: { UserCard },
  data() {
    return { message: "", show: false };
  },
  computed: {
    ...mapState({
      users: state => state.room.users
    }),
    ...mapGetters(["votes"])
  },
  mounted: function() {
    console.log("host mounted");
    this.CreateRoom();
  },
  methods: {
    ...mapActions(["CreateRoom", "SendMessage"]),
    Send() {
      this.show = false;
      this.SendMessage(this.message);
    }
  }
};
</script>
