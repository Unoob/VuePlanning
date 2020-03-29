<template>
  <v-hover v-slot:default="{ hover }">
    <v-container class="fill-height justify-center">
      <v-row class="text-center">
        <v-col cols="12" class="display-4 font-weight-bold"> {{ user.vote }} </v-col>
        <v-col cols="12" class="display-1" v-show="hover">{{ user.name }}</v-col>
      </v-row>
    </v-container>
  </v-hover>
</template>
<script>
export default {
  name: "UserCardFront",
  props: { user: { type: Object, default: () => ({}) } }
};
</script>
