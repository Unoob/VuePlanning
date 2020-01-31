<template>
  <component :is="View"></component>
</template>
<script>
import RoomHost from "@/components/RoomHost";
import RoomCards from "@/components/RoomCards";
import { mapState } from "vuex";
export default {
  name: "Room",
  components: { RoomHost, RoomCards },
  computed: {
    ...mapState({
      user: state => state.user
    }),
    View() {
      return `Room${this.user.isHost ? "Host" : "Cards"}`;
    }
  }
};
</script>
