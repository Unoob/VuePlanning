import { HubConnectionBuilder, LogLevel, HubConnectionState } from "@microsoft/signalr";
import { MessagePackHubProtocol } from "@microsoft/signalr-protocol-msgpack";
import store from "@/store/index";

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

export async function start() {
  try {
    await connection.start();
    console.log("connected");
  } catch (err) {
    console.log("start error", err);
    //setTimeout(() => start(), 5000);
  }
}

//connection.onclose(async () => {
//  await start();
//});

export function onFocus() {
  if (connection.state !== HubConnectionState.Disconnected) return;
  start().then(() => {
    const user = store.state.user;
    if (user.isHost) {
      connection.invoke("CreateRoom", user);
    } else {
      store.dispatch("JoinRoom", user);
    }
  });
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
  console.log("UserJoin", user);
  store.dispatch("UserJoin", user);
}

function UserLeaves(user) {
  store.dispatch("UserLeaves", user);
}

function UserVote(user) {
  store.dispatch("UserVote", user);
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
