import {UserComponentType, UserStatus} from '../Flag/TypeFlag';


export function CreateUserType(socket : SocketIO.Socket, name : string, student_id : string, 
                            status : UserStatus) : UserComponentType{ 

    return {
        socket_id : socket.id,
        socket : socket,
        name : name,
        student_id : student_id,
        room_id : "",
        type : status
    };
}