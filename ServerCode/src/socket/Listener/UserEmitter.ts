import {TeacherCreateMsgRoomType, TeacherCommonType, UserComponentType} from '../../Utility/Flag/TypeFlag';
import {UniversalSocketEvent} from '../../Utility/Flag/EventFlag';

export function EmitUserLeave(socket : SocketIO.Socket, userType : UserComponentType) {
    //Only emit to others, if room exist
    if (userType.room_id)
        socket.to(userType.room_id).emit(UniversalSocketEvent.UserLeaved, userType);
}