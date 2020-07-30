import {TeacherSocketEvent} from '../../Utility/Flag/EventFlag';
import {TeacherCreateMsgRoomType, TeacherCommonType} from '../../Utility/Flag/TypeFlag';

export function ListenUserEvent(socket : SocketIO.Socket) {

//#region Teacher Section
    socket.on(TeacherSocketEvent.CreateRoom, function (data : TeacherCreateMsgRoomType) {
        //TODO : Should force all student joined room
    });

    socket.on(TeacherSocketEvent.ForceEndGame, function (data : TeacherCommonType) {
        socket.to(data.room_id).emit(TeacherSocketEvent.ForceEndGame);
    });

    socket.on(TeacherSocketEvent.StartGame, function (data : TeacherCommonType) {
        //TODO : timer 40 min
        socket.to(data.room_id).emit(TeacherSocketEvent.StartGame);
    });

    socket.on(TeacherSocketEvent.Rally, function (data : TeacherCommonType) {
        //TODO: Don't know what is rally
        socket.to(data.room_id).emit(TeacherSocketEvent.Rally);
    });
//#endregion

//#region Student Section

//#endregion

//#region Universal Section

//#endregion

}