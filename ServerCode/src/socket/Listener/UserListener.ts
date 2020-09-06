import {TeacherSocketEvent, UniversalSocketEvent} from '../../Utility/Flag/EventFlag';
import {TeacherCreateMsgRoomType, TerminateEventType, TeacherCommonType, UserDataType, UserComponentType} from '../../Utility/Flag/TypeFlag';
import SocketEnvironment from '../SocketEnvironment';

export function ListenUserEvent(socket : SocketIO.Socket, socketServer : SocketIO.Server, socektEnv : SocketEnvironment) {
    let self = this;

//#region Teacher Section
    socket.on(TeacherSocketEvent.CreateRoom, function (data : string) {
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

    socket.on(TeacherSocketEvent.ForceEndGame, function (data : string) {
        let parseData : TerminateEventType = JSON.parse(data);

        socektEnv.RoomDismiss(parseData.room_id, parseData.location_id);
    });

    socket.on(TeacherSocketEvent.KickFromGame, function (data : string) {
        let parseData : UserComponentType = JSON.parse(data);
        socektEnv.LeaveRoom(parseData.user_id, parseData.room_id, parseData.type);
        
        socketServer.to(parseData.room_id).emit(TeacherSocketEvent.KickFromGame, data);
    });

    socket.on(TeacherSocketEvent.StartGame, function (data : string) {        
        let parseData : TeacherCommonType = JSON.parse(data);
        socektEnv.SetRoomTimer(parseData.room_id, Date.now());
        let roomComp = socektEnv.rooms.get(parseData.room_id);

        socketServer.to(parseData.room_id).emit(TeacherSocketEvent.StartGame, JSON.stringify(roomComp));
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
            
            let roomComp = socektEnv.rooms.get(userComp.room_id);
            roomComp.students = [];
            //If student is join after "game start", push this event
            if (roomComp.end_time > 0)
                socket.emit(TeacherSocketEvent.StartGame, JSON.stringify(roomComp));
        }
    });
//#endregion

}