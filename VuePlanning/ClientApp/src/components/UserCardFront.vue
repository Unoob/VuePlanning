<template>
  <v-hover v-slot:default="{ hover }">
    <v-container class="fill-height justify-center">
      <v-row class="text-center">
        <v-col cols="12" class="display-4 font-weight-bold"> {{ user.vote }} </v-col>
        <transition name="fade" mode="out-in">
          <v-col v-if="hover" cols="12" class="display-1 transition-fast-in-fast-out">
            {{ user.name }}
          </v-col>
        </transition>
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
