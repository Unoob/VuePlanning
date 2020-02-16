import Vue from "vue";
import Vuex from "vuex";
import { CreateRoom, JoinRoom, CardSelect, SendMessage, Disconnect } from "@/services/HubService";

Vue.use(Vuex);
const AVATAR_API = connectionId => `https://api.adorable.io/avatars/86/${connectionId}.png`;
const initialState = {
  user: null,
  room: {
    id: -1,
    message: "",
    host: {},
    users: []
  }
};
let store = new Vuex.Store({
  state: initialState,
  mutations: {
    SET_USER(state, user) {
      state.user = user;
    },
    SET_MESSAGE(state, message) {
      state.room.message = message;
      state.user.vote = null;
    },
    SET_USER_VOTE(state, vote) {
      state.user.vote = vote + "";
    },
    UPDATE_ROOM(state, { users }) {
      users.forEach(u => {
        u.avatar = AVATAR_API(u.connectionId);
        u.state = "Connected";
      });
      state.room.users = users;
    },
    SET_HOST(state, user) {
      state.room.id = user.roomId;
      state.room.host = user;
    },
    USER_JOIN(state, user) {
      var match = state.room.users.find(f => f.name === user.name);
      if (match) {
        match.connectionId = user.connectionId;
        match.avatar = AVATAR_API(user.connectionId);
        match.state = "Connected";
      } else {
        user.avatar = AVATAR_API(user.connectionId);
        user.state = "Connected";
        state.room.users.push(user);
      }
    },
    USER_LEAVES(state, user) {
      state.room.users = state.room.users.filter(u => u.connectionId != user.connectionId);
    },
    UPDATE_USER_VOTE(state, { connectionId, vote }) {
      let user = state.room.users.find(f => f.connectionId === connectionId);
      if (!user) return;
      user.vote = vote;
    },
    RESET_VOTES(state) {
      state.room.users.forEach(u => (u.vote = null));
    },
    USER_DISCONNECTED(state, connectionId) {
      var user = state.room.users.find(u => u.connectionId == connectionId);
      user.state = "Disconnected";
    }
  },
  actions: {
    SetMessage({ commit }, message) {
      commit("SET_MESSAGE", message);
    },
    SendMessage({ commit, state }, message) {
      SendMessage(state.room.id, message);
      commit("RESET_VOTES");
    },
    UserUpdate({ commit }, user) {
      if (user.isHost) {
        commit("SET_HOST", user);
      }
      commit("SET_USER", user);
    },
    UpdateRoom({ commit }, room) {
      commit("UPDATE_ROOM", room);
      commit("SET_MESSAGE", room.message);
    },
    UserJoin({ commit }, user) {
      commit("USER_JOIN", user);
    },
    UserLeaves({ commit }, user) {
      commit("USER_LEAVES", user);
    },
    UserVote({ commit }, { connectionId, vote }) {
      commit("UPDATE_USER_VOTE", { connectionId, vote });
    },
    CreateRoom({ state }) {
      CreateRoom(state.user);
    },
    JoinRoom({ state }) {
      JoinRoom(state.user);
    },
    CardSelect({ commit, state }, vote) {
      commit("SET_USER_VOTE", vote);
      CardSelect(state.user);
    },
    LeaveRoom({ state, commit }) {
      Disconnect(state.user);
      commit("SET_USER", null);
    },
    RemoveUser({ commit }, user) {
      Disconnect(user);
      commit("USER_LEAVES", user);
    },
    UserDisconnected({ commit }, connectionId) {
      commit("USER_DISCONNECTED", connectionId);
    }
  },
  modules: {}
});

export default store;
