import * as socket from 'socket.io';
import * as http from 'http';


export function SocketListen (app : http.Server) {
    const io = socket.listen(app);

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