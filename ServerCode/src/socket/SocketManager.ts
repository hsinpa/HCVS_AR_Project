import * as socket from 'socket.io';
import * as http from 'http';
import SocketEnvironment from './SocketEnvironment';
import SocketReconnectHelper from './SocketReconnectHelper';
import {UniversalSocketEvent} from '../Utility/Flag/EventFlag';
import {ReconnectRequestType} from '../Utility/Flag/TypeFlag';

import EventProcessor from '../Utility/EventProcessor';

import {ListenUserEvent} from './Listener/UserListener';
import UserEmitter from './Listener/UserEmitter';

//Paramter
const fps = 1;

export default class SocketManager {
    public env : SocketEnvironment;

    private io :socket.Server; 
    private eventProcesser : EventProcessor
    private reconnectHandler : SocketReconnectHelper;
    private userEmitter : UserEmitter;

    constructor(app : http.Server) {
        this.io = socket.listen(app);
        this.userEmitter = new UserEmitter(this.io);
        this.env = new SocketEnvironment(this.userEmitter);
        this.reconnectHandler = new SocketReconnectHelper(this, this.userEmitter, this.env); 
        //this.eventProcesser = new EventProcessor(this.env, this.io, fps);
        this.InitListener();
    }

    InitListener() {
        let self = this;
        this.io.sockets.on('connection', function (socket) {
            console.log(socket.id + " is connect");
            self.env.UserJoin(socket);
            
            ListenUserEvent(socket, self.io, self.env);

            //Send back basic server info when user first connected
            socket.emit("OnConnect", JSON.stringify({
                    socket_id : socket.id,
                })
            );
    
            socket.on(UniversalSocketEvent.Reconnect, function (data:string) {
                console.log("OnReconnect");
                let parseData : ReconnectRequestType = JSON.parse(data);
                self.reconnectHandler.RepairSocketID(parseData, socket);
                self.reconnectHandler.ReconnectUserProcess(socket);
            });

            //When client discconected
            socket.on('disconnect', function () {
                console.log(socket.id + " is disconnect");
                self.reconnectHandler.TempDisconnectUser(socket);
                let rootSocketID = self.reconnectHandler.GetPairSocketID(socket.id);
                let userComp = self.env.users.get(rootSocketID);

                if (userComp)
                    self.userEmitter.EmitUserLeave(userComp);
            
                // let userComp = self.env.UserDisconnect(socket.id);
                // self.userEmitter.EmitUserLeave(userComp);
                
               // self.Disonnect(socket.id);
            });
        });
    }

    //When client close their app entirely
    DiconnectAndCleanUp(socket_id : string) {
        let rootID = this.reconnectHandler.GetPairSocketID(socket_id);
        this.reconnectHandler.CleanUpSocketID(socket_id, rootID);
        this.Disonnect(rootID);
        this.Disonnect(socket_id);
    }

    Disonnect(socket_id : string) {
        let userComp = this.env.UserDisconnect(socket_id);
        if (userComp != null)
            this.userEmitter.EmitUserLeave(userComp);
    }

}