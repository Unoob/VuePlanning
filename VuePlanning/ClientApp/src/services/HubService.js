import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import store from "../store/index";

const connection = new HubConnectionBuilder()
  .withUrl("/PlanningHub")
  .configureLogging(LogLevel.Information)
  .build();

connection.on("UserUpdate", UserUpdate);
connection.on("UserJoin", UserJoin);
connection.on("UserVote", UserVote);
connection.on("UserLeaves", UserLeaves);
connection.on("RoomCreated", RoomCreated);
connection.on("SendMessage", SetMessage);

async function start() {
  try {
    await connection.start();
    console.log("connected");
  } catch (err) {
    console.log(err);
    setTimeout(() => start(), 5000);
  }
}

start();
connection.onclose(async () => {
  await start();
});
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
  connection.invoke("CreateRoom", user);
}

export function Disconnect(user) {
  connection.invoke("Disconnect", user);
}
