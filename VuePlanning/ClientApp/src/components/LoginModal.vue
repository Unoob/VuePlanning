<template>
<v-card :elevation="3" :loading="loading" :disabled="loading">
  <v-card-text>
    <v-text-field v-model="name"
                  :label="$t('login.name')"
                  prepend-icon="fa-user-circle"
                  type="text" />

    <v-text-field v-model="room"
                  :label="$t('login.room')"
                  :prepend-icon="roomIcon"
                  type="text" />
    {{name}}{{room}}
  </v-card-text>
  <v-card-actions>
    <v-spacer></v-spacer>
    <v-btn @click="onConfirm" color="primary">{{$t('login.confirm')}}</v-btn>
    <v-btn @click="onCreate" color="primary" outlined>{{$t('login.create')}}</v-btn>
  </v-card-actions>
</v-card>
</template>
<script>
  import { mapState } from 'vuex';
  import {CreateRoom} from'@/services/HubService'
  export default {
    name: 'LoginModal',
    data() {
      return {
        name: '',
        room: '',
        loading: false
      }
    },
    computed: {
      ...mapState(['status']),
      roomIcon() {
        return `fa-door-${(this.room ? "open" : "closed")}`;
      }
    },
    watch: {
      status(value) {
        if (!value) {
          this.loading = false;
          return
        };
        this.$router.push('room');
    
      }
    },
    methods: {
      onConfirm() {
        this.loading = true;
      },
      onCreate() {
        if (!(this.name || this.room)) return;
        this.loading = true;
        CreateRoom(this.name, this.room);
      }
    }
  }
</script>
