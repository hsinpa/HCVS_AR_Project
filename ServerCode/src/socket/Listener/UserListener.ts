import {TeacherSocketEvent, UniversalSocketEvent} from '../../Utility/Flag/EventFlag';
import {TeacherCreateMsgRoomType, TeacherCommonType, UserDataType} from '../../Utility/Flag/TypeFlag';
import SocketEnvironment from '../SocketEnvironment';

export function ListenUserEvent(socket : SocketIO.Socket, socketServer : SocketIO.Server, socektEnv : SocketEnvironment) {
    let self = this;

//#region Teacher Section
    socket.on(TeacherSocketEvent.CreateRoom, function (data : string) {
        //TODO : Should force all student joined room
        let parseData : TeacherCreateMsgRoomType = JSON.parse(data);
        let isSucess = socektEnv.CreateRoom(parseData.user_id, parseData.room_id, socket.id);

        if (isSucess) {
            socket.join(parseData.room_id);

            socektEnv.AutoJoinAllUserInClass(socket, parseData.room_id);
        }

        socket.emit(TeacherSocketEvent.CreateRoom, JSON.stringify({
            status : isSucess,
        }));
    });

    socket.on(TeacherSocketEvent.RefreshUserStatus, function() {
        let userComp = socektEnv.users.get(socket.id);

        if (socektEnv.rooms.has(userComp.room_id)) {
            let students = socektEnv.FindAllUserInClass(userComp.room_id);
            console.log(userComp.room_id);
            socket.emit(TeacherSocketEvent.RefreshUserStatus, JSON.stringify(students));
        }
    });

    socket.on(TeacherSocketEvent.ForceEndGame, function (data : TeacherCommonType) {
        socektEnv.RoomDismiss(data.room_id);
    });

    socket.on(TeacherSocketEvent.StartGame, function (data : string) {        
        let parseData : TeacherCommonType = JSON.parse(data);
        socektEnv.SetRoomTimer(parseData.room_id, Date.now());
        let roomComp = socektEnv.rooms.get(parseData.room_id);

        socketServer.to(parseData.room_id).emit(TeacherSocketEvent.StartGame, JSON.stringify(roomComp));
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

            socket.to(userComp.room_id).emit(UniversalSocketEvent.UserJoined, JSON.stringify(userComp));
        }
    });
//#endregion

}