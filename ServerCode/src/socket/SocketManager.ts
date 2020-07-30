import * as socket from 'socket.io';
import * as http from 'http';
import SocketEnvironment from './SocketEnvironment';
import EventProcessor from '../Utility/EventProcessor';

import {ListenUserEvent} from './Listener/UserListener';
import {EmitUserLeave} from './Listener/UserEmitter';

//Paramter
const fps = 30;

export default class SocketManager {
    public env : SocketEnvironment;

    private io :socket.Server; 
    private eventProcesser : EventProcessor

    constructor(app : http.Server) {
        this.io = socket.listen(app);
        this.env = new SocketEnvironment();
        this.eventProcesser = new EventProcessor(this.env, this.io, fps);
        this.InitListener();
    }

    InitListener() {
        let self = this;

        this.io.sockets.on('connection', function (socket) {
            console.log(socket.id + " is connect");
            self.env.UserJoin(socket);
            
            ListenUserEvent(socket, self.env);

            //Send back basic server info when user first connected
            socket.emit("OnConnect", JSON.stringify({
                    socket_id : socket.id,
                })
            );
    
            //When client discconected
            socket.on('disconnect', function () {
                console.log(socket.id + " is disconnect");
                let userComp = self.env.UserDisconnect(socket.id);
                EmitUserLeave(socket, userComp);
            });
        });
    }


}