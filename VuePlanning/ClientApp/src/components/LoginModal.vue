<template>
<v-card :elevation="3">
  <v-card-text>
    <v-text-field @input="name=$event"
                  :label="$t('login.name')"
                  prepend-icon="fa-user-circle"
                  type="text" />

    <v-text-field @input="room=$event"
                  :label="$t('login.room')"
                  prepend-icon="fab fa-stack-exchange"
                  type="text" />
  </v-card-text>
  <v-card-actions>
    <v-spacer></v-spacer>
    <v-btn color="primary">{{$t('login.confirm')}}</v-btn>
    <v-btn color="primary" outlined>{{$t('login.create')}}</v-btn>
  </v-card-actions>
</v-card>
</template>
<script>
  export default {
    name: 'LoginModal',
    data(){
      return {
        name: '',
        room:''
      }
  }
  }
</script>
