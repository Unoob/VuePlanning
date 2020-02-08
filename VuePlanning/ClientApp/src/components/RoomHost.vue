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
        <v-card height="250" :color="show ? 'success lighten-3' : 'primary'" :dark="!show">
          <v-scroll-x-transition>
            <div v-if="!show">
              <v-card-title class="pb-0 justify-end">
                <v-tooltip left>
                  <template v-slot:activator="{ on }">
                    <v-btn @click="RemoveUser(user)" icon v-on="on">
                      <v-icon>fas fa-times</v-icon>
                    </v-btn>
                  </template>
                  <span>{{ $t("room.remove") }}</span>
                </v-tooltip>
              </v-card-title>
              <v-container class="text-capitalize text-center">
                <v-row>
                  <v-col cols="12" class="">
                    <v-avatar color="primary" size="86">
                      <v-img :src="user.avatar"></v-img>
                    </v-avatar>
                  </v-col>
                  <v-col cols="12" class="headline" :style="{ color: user.vote ? '' : 'gold' }">
                    {{ user.name }}
                  </v-col>
                </v-row>
              </v-container>
            </div>
          </v-scroll-x-transition>
          <v-scroll-x-transition>
            <v-container v-if="show" class="fill-height justify-center">
              <v-row>
                <v-col class="display-3 text-center font-weight-bold">
                  {{ user.vote }}
                </v-col>
              </v-row>
            </v-container>
          </v-scroll-x-transition>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapState, mapActions } from "vuex";
export default {
  name: "Host",
  mounted: function() {
    console.log("host mounted");
    this.CreateRoom();
  },
  data() {
    return { message: "", show: false };
  },
  computed: mapState({
    users: state => state.room.users
  }),
  methods: {
    ...mapActions(["CreateRoom", "SendMessage", "RemoveUser"]),
    Send() {
      this.show = false;
      this.SendMessage(this.message);
    }
  }
};
</script>
