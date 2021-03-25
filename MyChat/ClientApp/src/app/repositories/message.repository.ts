import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chat } from '../models/chat';
import { Message } from '../models/message';

@Injectable()
export class MessageRepository {
    private url = "/api/chats/";

    public constructor(private http: HttpClient) { }

    public getChatMessages(chat: Chat) {
        return this.http.get(this.url + chat.id + '/messages');
    }

    public addChatMessage(chat: Chat, message: Message) {
        return this.http.post(this.url + chat.id + '/messages', message);
    }
}