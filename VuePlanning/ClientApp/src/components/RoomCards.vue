<template>
  <v-container>
    <v-row><v-textarea :value="message" rows="1" readonly outlined auto-grow></v-textarea></v-row>
    <v-row>
      <v-col lg="3" md="4" cols="6" v-for="card in cards" :key="card">
        <v-card @click="CardSelect(card)" :color="`${selected==card?'green':'grey lighten-3'}`">
          <v-card-text class="text-center headline">{{ card }}</v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import { mapActions, mapState } from "vuex";
  export default {
    name: "Cards",
    mounted: function () {
      this.JoinRoom();
    },
    data() {
      return {
        cards: ["1/2"].concat(this.fib(9))
      };
    },
    computed: mapState({
      message: state => state.room.message,
      selected: state => state.user.vote
    }),
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
