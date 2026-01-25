import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalRClient {
  public hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5270/StockValuesHub")
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start()
    .then(() => console.log("SignalR hub connected"))
    .catch(err => console.log("Failed to connect to SignalR hub", err));
  }
}
