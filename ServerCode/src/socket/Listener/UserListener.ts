import {TeacherSocketEvent, UniversalSocketEvent} from '../../Utility/Flag/EventFlag';
import {TeacherCreateMsgRoomType, TeacherCommonType, UserDataType} from '../../Utility/Flag/TypeFlag';
import SocketEnvironment from '../SocketEnvironment';

export function ListenUserEvent(socket : SocketIO.Socket, socektEnv : SocketEnvironment) {

    let self = this;
//#region Teacher Section
    socket.on(TeacherSocketEvent.CreateRoom, function (data : TeacherCreateMsgRoomType) {
        //TODO : Should force all student joined room

        let isSucess = socektEnv.CreateRoom(data.user_id, data.room_id);

        socket.emit(TeacherSocketEvent.CreateRoom, JSON.stringify({status : isSucess}));
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
    socket.on(UniversalSocketEvent.UpdateUserInfo, function (data : string) {
        let parseData : UserDataType = JSON.parse(data);

        let userComp = socektEnv.UpdateUserLoginInfo(socket.id, parseData.user_name, parseData.user_id, parseData.room_id, parseData.userType);

        if (userComp && socektEnv.CheckIfRoomAvailable(userComp)) {
            socket.join(userComp.room_id);

            socket.to(userComp.room_id).emit(UniversalSocketEvent.UserJoined, userComp);
        }
    });
//#endregion

}