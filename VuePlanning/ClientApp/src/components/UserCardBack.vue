<template>
  <div>
    <v-card-title class="pb-0 justify-end">
      <v-tooltip left>
        <template v-slot:activator="{ on }">
          <v-btn icon @click="$emit('remove', user)" v-on="on">
            <v-icon>fas fa-times</v-icon>
          </v-btn>
        </template>
        <span>{{ $t("room.remove") }}</span>
      </v-tooltip>
    </v-card-title>
    <v-container class="text-capitalize text-center">
      <v-row>
        <v-col cols="12" class="">
          <v-avatar color="accent" size="86">
            <v-icon v-if="!user.avatar" size="64" dark>fa-user-circle</v-icon>
            <v-img :src="user.avatar"></v-img>
          </v-avatar>
        </v-col>
        <v-col cols="12" class="headline font-weight-bold" :style="styles">
          {{ user.name }}
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>
<script>
export default {
  name: "UserCardBack",
  props: {
    user: {
      type: Object,
      default: () => ({})
    }
  },
  computed: {
    styles() {
      const { vote, userState } = this.user;
      return {
        color: vote ? "white" : "gold",
        textDecoration: userState === 1 ? "line-through" : ""
      };
    }
  }
};
</script>
