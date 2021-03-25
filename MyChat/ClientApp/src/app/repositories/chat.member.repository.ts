import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chat } from '../models/chat';
import { User } from '../models/user';

@Injectable()
export class ChatMemberRepository {
    private url = "/api/chats";

    public constructor(private http: HttpClient) { }

    public getChatMembers(chat: Chat) {
        return this.http.get(this.url + '/' + chat.id + '/members');
    }

    public getChatMember(chat: Chat, user: User) {
        return this.http.get(this.url + '/' + chat.id + '/members' + user.id);
    }

    public addChatMember(chat: Chat, user: User) {
        return this.http.post(this.url + '/' + chat.id + '/members', user);
    }

    public deleteChatMember(chat: Chat, user: User) {
        return this.http.delete(this.url + '/' + chat.id + '/members/' + user.id);
    }
}