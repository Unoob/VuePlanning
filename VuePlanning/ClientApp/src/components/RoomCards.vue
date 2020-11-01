<template>
  <v-container>
    <v-row>
      <v-textarea
        :value="message"
        rows="1"
        readonly
        outlined
        auto-grow
      ></v-textarea>
      <NotificationSwitch></NotificationSwitch>
    </v-row>
    <v-row>
      <v-col v-for="(card, i) in cards" :key="card" lg="3" md="4" cols="6">
        <v-card
          :color="`${selected == card ? 'green' : 'grey lighten-3'}`"
          @click="CardSelect(card)"
        >
          <v-card-text>
            <div>{{ i }}</div>
            <p class="text-center headline">{{ card }}</p>
          </v-card-text>
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
  created: function() {
    document.addEventListener("keyup", this.numpad);
  },
  mounted: function() {
    console.log("card mounted");
    this.JoinRoom();
  },
  destroyed: function() {
    document.removeEventListener("keyup", this.numpad);
  },
  methods: {
    ...mapActions(["JoinRoom", "CardSelect"]),
    fib(n) {
      let arr = [0, 1];
      for (let i = 2; i < n + 2; i++) {
        arr.push(arr[i - 2] + arr[i - 1]);
      }
      return arr.slice(2);
    },
    numpad(e) {
      console.log(e);
      const index = parseInt(e.key);
      console.log(index);
      if (isNaN(index)) return;
      this.CardSelect(this.cards[index]);
    }
  }
};
</script>
