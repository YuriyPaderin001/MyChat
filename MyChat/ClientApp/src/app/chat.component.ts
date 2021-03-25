import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { DataService } from './services/data.service';
import { SignalRService } from './services/signalr.service';
import { Chat } from './models/chat';
import { User } from './models/user';

export enum ChatComponentState {
    ChatListShowing,
    ChatCreating,
    ChatEditing
};

@Component({
    selector: 'chat-component',
    templateUrl: './chat.component.html',
    providers: [DataService, SignalRService]
})
export class ChatComponent implements OnInit {
    @Input() public user: User;
    @Output() public onChatSelected = new EventEmitter<Chat>();
    @Output() public onLogout = new EventEmitter();


    public State = ChatComponentState;
    public currentState = ChatComponentState.ChatListShowing;

    public chat: Chat = new Chat();
    public chats: Chat[] = [];
    public users: User[] = [];
    public chatMembers: User[] = [];
    public membersToAdd: User[] = [];
    public membersToDelete: User[] = [];

    constructor(private dataService: DataService, private signalRService: SignalRService) { }

    public ngOnInit() {
        // Реализовать отображение последнего сообщения в списке как в ВК
        // this.signalRService.startConnection();
        // this.signalRService.addReceiveMessageListener((data: Message) => this.messages.push(data));

        this.loadChats();
    }


    private loadChats() {
        this.dataService.chats.getChats()
            .subscribe((data: Chat[]) => this.chats = data);
    }

    private loadUsers() {
        this.dataService.users.getUsers()
            .subscribe((data: User[]) => this.users = data);
    }

    private loadChatMembers() {
        this.dataService.chatMembers.getChatMembers(this.chat)
            .subscribe((data: User[]) => this.chatMembers = data);
    }

    public createChat() {
        this.dataService.chats.addChat(this.chat)
            .subscribe((generatedId: number) => {
                if (generatedId > 0) {
                    this.chat.id = generatedId;
                    for (let member of this.membersToAdd) {
                        this.dataService.chatMembers.addChatMember(this.chat, member)
                            .subscribe();
                    }

                    this.loadChats();
                    this.showChatListPage();
                }
            });
    }

    public updateChat() {
        this.dataService.chats.updateChat(this.chat)
            .subscribe((updatedRowsCount: number) => {
                for (let member of this.membersToAdd) {
                    this.dataService.chatMembers.addChatMember(this.chat, member)
                        .subscribe((data) => { });
                }

                for (let member of this.membersToDelete) {
                    this.dataService.chatMembers.deleteChatMember(this.chat, member)
                        .subscribe((data) => { });
                }

                this.loadChats();
                this.showChatListPage();
            });
    }

    public leaveChat(chat: Chat) {
        this.dataService.chatMembers.deleteChatMember(chat, this.user)
            .subscribe((deletedRowsCount: number) => {
                if (deletedRowsCount > 0) {
                    this.loadChats()
                }
            });
    }

    public markUserToAdd(user: User) {
        this.membersToDelete = this.membersToDelete.filter((usr: User) => usr.id != user.id);
        console.log('membersToDelete');
        console.log(this.membersToDelete);

        console.log('isChatMember ' + this.isChatMember(user));
        console.log('hasUserMarkToAdd ' + this.hasUserMarkToAdd(user));
        if (!this.hasUserMarkToAdd(user) && (!this.isChatMember(user) || this.hasUserMarkToDelete(user))) {
            this.membersToAdd.push(user);
        }
    }

    public markUserToDelete(user: User) {
        this.membersToAdd = this.membersToAdd.filter((usr: User) => usr.id != user.id);
        console.log('membersToAdd');
        console.log(this.membersToAdd);

        if (!this.hasUserMarkToDelete(user) && (this.isChatMember(user) || this.hasUserMarkToAdd(user))) {
            this.membersToDelete.push(user);
            console.log(this.membersToDelete);
        }
    }

    public isSelectedUser(user: User) {
        return ((this.isChatMember(user) && !this.hasUserMarkToDelete(user)) || this.hasUserMarkToAdd(user));
    }

    public isChatMember(user: User) {
        return this.chatMembers.some((usr: User) => usr.id == user.id);
    }

    public hasUserMarkToAdd(user: User) {
        return this.membersToAdd.some((usr: User) => usr.id == user.id);
    }

    public hasUserMarkToDelete(user: User) {
        return this.membersToDelete.some((usr: User) => usr.id == user.id);
    }

    public showChatListPage() {
        this.setState(ChatComponentState.ChatListShowing);
    }

    public showChatCreationPage() {
        this.loadUsers();
        this.chat = new Chat();
        this.membersToAdd = [];
        this.setState(ChatComponentState.ChatCreating);
    }

    public showChatEditionPage(selectedChat: Chat) {
        this.chat = selectedChat;
        this.loadUsers();
        this.loadChatMembers();
        this.membersToAdd = [];
        this.membersToDelete = [];
        this.setState(ChatComponentState.ChatEditing);
    }

    private setState(state: ChatComponentState) {
        this.currentState = state;
    }

    private selectChat(chat: Chat) {
        this.onChatSelected.emit(chat);
    }

    private logout() {
        this.onLogout.emit();
    }
}