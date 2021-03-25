import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {
    public data: Object;

    private hubConnection: SignalR.HubConnection;

    constructor() { }

    public startConnection() {
        this.hubConnection = new SignalR.HubConnectionBuilder()
            .withUrl('https://localhost:44328/api/hubs/chatmessages')
            .build();

        this.hubConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch(err => console.log('Error while starting connection: ' + err))
    }

    public addReceiveMessageListener(listener) {
        this.hubConnection.on('receiveMessage', listener);
    }
}