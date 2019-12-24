import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import store from '../store/index';

const connection = new HubConnectionBuilder()
  .withUrl("/PlanningHub")
  .configureLogging(LogLevel.Information)
  .build();

connection.on('SetStatus',SetStatus)

async function start() {
  try {
    await connection.start();
    console.log("connected");
  } catch (err) {
    console.log(err);
    setTimeout(() => start(), 5000);
  }
};

start();

function SetStatus(status) {
  console.log("SetStatus", status);
  store.commit('SetStatus', status);
}

export function CreateRoom(name, room) {
  console.log('CreateRoom', name, room);
  connection.invoke('CreateRoom', { name, room });
}
