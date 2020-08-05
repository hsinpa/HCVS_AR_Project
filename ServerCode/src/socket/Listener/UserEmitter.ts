import {TeacherCreateMsgRoomType, TeacherCommonType, UserComponentType} from '../../Utility/Flag/TypeFlag';
import {UniversalSocketEvent, TeacherSocketEvent} from '../../Utility/Flag/EventFlag';


export default class UserEmitter {

    _socket : SocketIO.Server;

    constructor(socket : SocketIO.Server) {
        this._socket = socket;
    }

    EmitUserJoinRoom(s : SocketIO.Socket, room_id : string, userComp : UserComponentType) {
        s.join(room_id);
        s.to(room_id).emit(UniversalSocketEvent.UserJoined, JSON.stringify(userComp));
    }

    EmitUserLeave(userType : UserComponentType) {
        //Only emit to others, if room exist
        if (userType.room_id)
            this._socket.to(userType.room_id).emit(UniversalSocketEvent.UserLeaved, JSON.stringify(userType));
    }

    EmitForceLeave(room_id : string, location_id : string) {
        //Only emit to others, if room exist
        if (room_id)
            this._socket.to(room_id).emit(TeacherSocketEvent.ForceEndGame,
                JSON.stringify({location_id : location_id})
            );
    }
}