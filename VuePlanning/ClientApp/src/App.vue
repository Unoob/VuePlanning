<template>
  <v-app>
      <router-view></router-view>
  </v-app>
</template>

<script>
import "./services/HubService";
export default {
  name: "App"
};
</script>
