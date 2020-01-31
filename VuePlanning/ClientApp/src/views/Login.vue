<template>
  <v-container class="fill-height">
    <v-row align="center" justify="center">
      <v-col lg="4" md="6">
        <LoginModal></LoginModal>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
import LoginModal from "@/components/LoginModal";
export default {
  name: "Login",
  components: { LoginModal }
};
</script>
