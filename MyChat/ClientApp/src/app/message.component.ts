import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { DataService } from './services/data.service';
import { SignalRService } from './services/signalr.service';
import { Attachment } from './models/attachment';
import { Chat } from './models/chat';
import { Message } from './models/message';
import { User } from './models/user';

export enum MessageComponentState {
    MessageListShowing
}

@Component({
    selector: 'message-component',
    templateUrl: './message.component.html',
    providers: [DataService, SignalRService]
})
export class MessageComponent implements OnInit {
    @Input() public user: User;
    @Input() public chat: Chat;
    @Output() public onExit = new EventEmitter();
    @Output() public onLogout = new EventEmitter();

    public State = MessageComponentState;
    public currentState = MessageComponentState.MessageListShowing;

    public message: Message = new Message();
    public selectedFiles: File[] = [];

    public messages: Message[];
    public attachmentsMap: Map<number, Attachment[]> = new Map();
    public sendersMap: Map<number, User> = new Map();

    constructor(private dataService: DataService, private signalRService: SignalRService) { }

    public ngOnInit() {
        this.signalRService.startConnection();
        this.signalRService.addReceiveMessageListener((data: Message) => this.loadChatMessages());

        this.loadChatMessages();
    }

    public loadChatMessages() {
        this.dataService.messages.getChatMessages(this.chat)
            .subscribe((data: Message[]) => {
                this.messages = data;
                this.messages.forEach((msg: Message) => {
                    this.loadMessageSender(msg);
                    this.loadMessageAttachments(msg);
                });
            });
    }

    public loadMessageSender(msg: Message) {
        if (!this.sendersMap.has(msg.senderId)) {
            this.dataService.users.getUserById(msg.senderId)
                .subscribe((sender: User) => {
                    if (sender != null) {
                        this.sendersMap.set(msg.senderId, sender);
                    }
                });
        }
    }

    public loadMessageAttachments(msg: Message) {
        this.dataService.attachments.getMessageAttachments(msg)
            .subscribe((attachments: Attachment[]) => {
                if (attachments != null && attachments.length > 0) {
                    this.attachmentsMap.set(msg.id, attachments);
                }
            });
    }

    public sendMessage() {
        this.dataService.messages.addChatMessage(this.chat, this.message)
            .subscribe((messageId: number) => {
                if (messageId > 0) {
                    for (let file of this.selectedFiles) {
                        this.dataService.attachments.addMessageAttachmentByMessageId(messageId, file)
                            .subscribe((attachmentId: Attachment) => {
                                if (attachmentId > 0) {
                                    this.message.id = messageId;
                                    this.loadMessageAttachments(this.message);
                                }
                            });
                    }

                    this.selectedFiles = [];
                    this.loadChatMessages();
                }
            });

        this.message = new Message();
    }

    public onFileSelected(files: FileList) {
        for (let i = 0; i < files.length; i++) {
            this.selectedFiles.push(files[i]);
        }
    }

    public exit() {
        this.onExit.emit();
    }

    public logout() {
        this.onLogout.emit();
    }

    public showMessageList() {
        this.message = new Message();
        this.selectedFiles = [];
        this.setState(MessageComponentState.MessageListShowing);
    }

    private setState(state: MessageComponentState) {
        this.currentState = state;
    }
}