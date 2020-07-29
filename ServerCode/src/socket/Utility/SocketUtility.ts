import {UserComponentType, UserStatus} from '../Flag/TypeFlag';


export function CreateUserType(socket : SocketIO.Socket) : UserComponentType{ 
    return {
        socket_id : socket.id,
        name : "",
        user_id : "",
        room_id : "",
        type : UserStatus.Guest
    };
}