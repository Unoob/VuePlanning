<template>
  <v-content>
    <v-toolbar color="primary"></v-toolbar>
    <router-view></router-view>
  </v-content>
</template>
<script>
  export default {
    name: "MasterView",
    mounted: function () {
      this.$nextTick(() => {
        //window.addEventListener('focus', () => alert('ny'))
      })
    }
  };
</script>
