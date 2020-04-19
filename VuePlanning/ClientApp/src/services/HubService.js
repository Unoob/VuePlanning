import { HubConnectionBuilder, LogLevel, HubConnectionState } from "@microsoft/signalr";
import { MessagePackHubProtocol } from "@microsoft/signalr-protocol-msgpack";
import store from "@/store/index";

function _setConnectionState(state) {
  store.dispatch("SetConnectionState", state);
}

const connection = new HubConnectionBuilder()
  .withUrl("/PlanningHub")
  .withHubProtocol(new MessagePackHubProtocol())
  .configureLogging(LogLevel.Information)
  .build();

connection.on("UserUpdate", UserUpdate);
connection.on("UserJoin", UserJoin);
connection.on("UserVote", UserVote);
connection.on("UserLeaves", UserLeaves);
connection.on("RoomCreated", RoomCreated);
connection.on("SendMessage", SetMessage);
connection.on("UserDisconnected", UserDisconnected);
connection.on("ChangeRoomUserState", ChangeRoomUserState);

export async function start() {
  if (connection.state !== HubConnectionState.Disconnected) return Promise.reject(connection.state);
  try {
    const promise = await connection.start();
    console.log("connected");
    _setConnectionState(HubConnectionState.Connected);
    return promise;
  } catch (err) {
    console.log("start error", err);
    setTimeout(() => start(), 5000);
  }
}

connection.onreconnecting(() => {
  _setConnectionState(HubConnectionState.Reconnecting);
});
connection.onclose(() => {
  _setConnectionState(HubConnectionState.Disconnected);
});

export function onFocus() {
  start().then(
    () => {
      const user = store.state.user;
      if (user.isHost) {
        connection.invoke("CreateRoom", user);
      } else {
        store.dispatch("JoinRoom", user);
      }
    },
    connectionState => {
      console.log("Current state:", connectionState);
    }
  );
}

function UserDisconnected(connectionId) {
  store.dispatch("UserDisconnected", connectionId);
}

function UserUpdate(user) {
  store.dispatch("UserUpdate", user);
}
function RoomCreated(room) {
  store.dispatch("UpdateRoom", room);
}

function SetMessage(msg) {
  store.dispatch("SetMessage", msg);
}

function UserJoin(user) {
  store.dispatch("UserJoin", user);
}

function UserLeaves(user) {
  store.dispatch("UserLeaves", user);
}

function UserVote(user) {
  store.dispatch("UserVote", user);
}

function ChangeRoomUserState(user) {
  store.dispatch("SetRoomUserState", user);
}

export function ChangeUserState(user) {
  connection.invoke("ChangeUserState", user);
}

export function SendMessage(roomId, msg) {
  connection.invoke("SendMessage", roomId, msg);
}

export function JoinRoom(user) {
  connection.invoke("JoinRoom", user);
}
export function CardSelect(user) {
  connection.invoke("CardSelect", user);
}
export function CreateRoom(user) {
  connection.invoke("CreateRoom", user).catch(e => {
    console.log("CreateRoom error", e);
  });
}

export function Disconnect(user) {
  connection.invoke("Disconnect", user);
}

export function SaveUserSubscription(userSubscription) {
  connection.invoke("SaveUserSubscription", userSubscription);
}
