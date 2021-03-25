import { Component, OnInit } from '@angular/core';

import { DataService } from './services/data.service';
import { SignalRService } from './services/signalr.service';
import { Chat } from './models/chat';
import { User } from './models/user';

export enum AppComponentState {
    AccountComponentShowing,
    ChatComponentShowing,
    MessageComponentShowing
}

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    providers: [ DataService, SignalRService ]
})
export class AppComponent implements OnInit {
    public State = AppComponentState;
    public currentState: AppComponentState = AppComponentState.AccountComponentShowing;

    public authorizatedUser: User = new User();
    public selectedChat: Chat = new Chat();

    constructor(private dataService: DataService, private signalRService: SignalRService) { }

    public ngOnInit() {
        
    }

    public logout() {
        this.authorizatedUser = new User();
        this.dataService.session.logout();
        this.showAccountComponent();
    }

    public onUserAuthorizated(user: User) {
        this.authorizatedUser = user;
        this.showChatComponent();
    }

    public onChatSelected(chat: Chat) {
        this.selectedChat = chat;
        this.showMessageComponent();
    }

    public onExitFromChat() {
        this.selectedChat = new Chat();
        this.showChatComponent();
    }

    public showAccountComponent() {
        this.authorizatedUser = new User();
        this.setState(AppComponentState.AccountComponentShowing);
    }

    public showChatComponent() {
        this.setState(AppComponentState.ChatComponentShowing);
    }

    public showMessageComponent() {
        this.setState(AppComponentState.MessageComponentShowing);
    }

    private setState(newState: AppComponentState) {
        this.currentState = newState;
    }
}
