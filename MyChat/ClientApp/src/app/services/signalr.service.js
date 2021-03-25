"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SignalRService = void 0;
var SignalR = require("@aspnet/signalr");
// @Injectable()
var SignalRService = /** @class */ (function () {
    function SignalRService(url) {
        var _this = this;
        this.url = url;
        this.startConnection = function () {
            _this.hubConnection = new SignalR.HubConnectionBuilder()
                .withUrl(_this.url)
                .build();
            _this.hubConnection
                .start()
                .then(function () { return console.log('Connection started'); })
                .catch(function (err) { return console.log('Error while starting connection: ' + err); });
        };
        this.addDataListener = function () {
            _this.hubConnection.on('Notify', function (data) {
                _this.data = data;
                console.log(JSON.stringify(data));
            });
        };
    }
    return SignalRService;
}());
exports.SignalRService = SignalRService;
//# sourceMappingURL=signalr.service.js.map