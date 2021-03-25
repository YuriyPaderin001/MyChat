export class Message {
    constructor(
        public id?: number,
        public chatId?: number,
        public senderId?: number,
        public content?: string,
        public sendingDateTime?: Date) { }
}