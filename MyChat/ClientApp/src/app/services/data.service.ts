import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { AttachmentRepository } from '../repositories/attachment.repository';
import { ChatRepository } from '../repositories/chat.repository';
import { ChatMemberRepository } from '../repositories/chat.member.repository';
import { MessageRepository } from '../repositories/message.repository';
import { SessionRepository } from '../repositories/session.repository';
import { UserRepository } from '../repositories/user.repository';

import { LoginData } from '../models/login.data';
import { User } from '../models/user';

@Injectable()
export class DataService {
    public readonly attachments: AttachmentRepository;
    public readonly chats: ChatRepository;
    public readonly chatMembers: ChatMemberRepository;
    public readonly messages: MessageRepository;
    public readonly users: UserRepository;
    public readonly session: SessionRepository;

    constructor(private http: HttpClient) {
        this.attachments = new AttachmentRepository(http);
        this.chats = new ChatRepository(http);
        this.chatMembers = new ChatMemberRepository(http);
        this.messages = new MessageRepository(http);
        this.session = new SessionRepository(http);
        this.users = new UserRepository(http);
    }
}