import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chat } from '../models/chat';
import { User } from '../models/user';

@Injectable()
export class ChatRepository {
    private url = "/api/chats";

    public constructor(private http: HttpClient) { }

    public getChats() {
        return this.http.get(this.url);
    }

    public getChat(id: number) {
        return this.http.get(this.url + '/' + id);
    }

    public addChat(chat: Chat) {
        return this.http.post(this.url, chat);
    }

    public addUserToChat(user: User, chat: Chat) {
        return this.http.post(this.url + '/' + chat.id + '/members', user);
    }

    public updateChat(chat: Chat) {
        return this.http.put(this.url, chat);
    }

    public deleteChat(chat: Chat) {
        return this.http.delete(this.url + '/' + chat.id);
    }

    public removeUserFromChat(user: User, chat: Chat) {
        return this.http.delete(this.url + '/' + chat.id + '/members/' + user.id);
    }
}