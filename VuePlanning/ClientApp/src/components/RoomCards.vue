<template>
  <v-container>
    <v-row>
      <v-textarea :value="message" rows="1" readonly outlined auto-grow></v-textarea>
      <NotificationSwitch></NotificationSwitch>
    </v-row>
    <v-row>
      <v-col v-for="card in cards" :key="card" lg="3" md="4" cols="6">
        <v-card
          :color="`${selected == card ? 'green' : 'grey lighten-3'}`"
          @click="CardSelect(card)"
        >
          <v-card-text class="text-center headline">{{ card }}</v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapActions, mapState } from "vuex";
import NotificationSwitch from "./NotificationSettings";
export default {
  name: "Cards",
  components: { NotificationSwitch },
  data() {
    return {
      cards: ["½"].concat(this.fib(9))
    };
  },
  computed: mapState({
    message: state => state.room.message,
    selected: state => state.user.vote
  }),
  mounted: function() {
    console.log("card mounted");
    this.JoinRoom();
  },
  methods: {
    ...mapActions(["JoinRoom", "CardSelect"]),
    fib(n) {
      let arr = [0, 1];
      for (let i = 2; i < n + 2; i++) {
        arr.push(arr[i - 2] + arr[i - 1]);
      }
      return arr.slice(2);
    }
  }
};
</script>
