"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Message = void 0;
var Message = /** @class */ (function () {
    function Message(id, chatId, senderId, content, sendingDateTime) {
        this.id = id;
        this.chatId = chatId;
        this.senderId = senderId;
        this.content = content;
        this.sendingDateTime = sendingDateTime;
    }
    return Message;
}());
exports.Message = Message;
//# sourceMappingURL=message.js.map