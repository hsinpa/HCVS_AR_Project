import * as socket from 'socket.io';
import * as http from 'http';
import EnvironmentComponent from './Components/EnvironmentComponent';
import EventProcessor from './Utility/EventProcessor';

//Paramter
const fps = 30;

export function SocketListen (app : http.Server) {
    const io = socket.listen(app);
    const env = new EnvironmentComponent();
    const eventProcesser = new EventProcessor(env, io, fps);

    io.sockets.on('connection', function (socket) {
        console.log(socket.id + " is connect");

        //Send back basic server info when user first connected
        socket.emit("OnConnect", JSON.stringify({
                socket_id : socket.id,
            })
        );

        //When client discconected
        socket.on('disconnect', function () {
            console.log(socket.id + " is disconnect");
        });
    });
};