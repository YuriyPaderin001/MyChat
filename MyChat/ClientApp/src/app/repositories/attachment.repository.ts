import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Message } from '../models/message';

@Injectable()
export class AttachmentRepository {
    private url = "api/messages/";

    public constructor(private http: HttpClient) { }

    public getMessageAttachments(message: Message) {
        return this.getMessageAttachmentsByMessageId(message.id);
    }

    public getMessageAttachmentsByMessageId(id: number) {
        return this.http.get(this.url + id + '/attachments');
    }

    public getMessageAttachment(messageId: number, id: number) {
        return this.http.get(this.url + messageId + '/attachments/' + id);
    }

    public addMessageAttachment(message: Message, file: File) {
        return this.addMessageAttachmentByMessageId(message.id, file);
    }

    public addMessageAttachmentByMessageId(messageId: number, file: File) {
        let formData: FormData = new FormData();
        formData.append('file', file);
        return this.http.post(this.url + messageId + '/attachments', formData);
    }
}