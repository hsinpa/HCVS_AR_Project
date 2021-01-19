import {TeacherSocketEvent, UniversalSocketEvent} from '../../Utility/Flag/EventFlag';
import {TeacherCreateMsgRoomType, TerminateEventType, TeacherCommonType, UserDataType, UserComponentType, AccessTokenType} from '../../Utility/Flag/TypeFlag';
import SocketEnvironment from '../SocketEnvironment';
import { Socket,Server } from 'socket.io';

export function ListenUserEvent(socket : Socket, socketServer : Server, socektEnv : SocketEnvironment) {
    let self = this;

//#region Teacher Section
    socket.on(TeacherSocketEvent.CreateRoom, function (data : string) {
        let parseData : TeacherCreateMsgRoomType = JSON.parse(data);
        let isSucess = socektEnv.CreateRoom(parseData.user_id, parseData.user_name, parseData.room_id, parseData.root_socket_id);

        if (isSucess) {
            socket.join(parseData.room_id);

            socektEnv.AutoJoinAllUserInClass(socket, parseData.room_id);
        }
        
        console.log("CreateRoom " + data);
        socket.emit(TeacherSocketEvent.CreateRoom, JSON.stringify({
            status : isSucess,
        }));
    });

    socket.on(TeacherSocketEvent.RefreshUserStatus, function(data : string) {
        let parseData : AccessTokenType = JSON.parse(data);
        let userComp = socektEnv.users.get(parseData.socket_id);

        if (userComp && userComp.room_id && socektEnv.rooms.has(userComp.room_id)) {
            let students = socektEnv.FindAllUserInClass(userComp.room_id);
            console.log(userComp.room_id);
            socket.emit(TeacherSocketEvent.RefreshUserStatus, JSON.stringify(students));
        }
    });

    socket.on(TeacherSocketEvent.ForceEndGame, function (data : string) {
        let parseData : TerminateEventType = JSON.parse(data);

        console.log(TeacherSocketEvent.ForceEndGame +", parseData.room_id " + parseData.room_id);
        socektEnv.RoomDismiss(parseData.room_id, parseData.location_id);

        if (parseData.room_id && parseData.location_id)
            socektEnv.cacheLastRoomHistory.set(parseData.room_id, parseData.location_id);
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
        console.log("Start game " + parseData.room_id);
        socketServer.to(parseData.room_id).emit(TeacherSocketEvent.StartGame, JSON.stringify(roomComp));
    });
//#endregion

//#region Student Section

//#endregion

//#region Universal Section
    socket.on(UniversalSocketEvent.UpdateUserInfo, function (data : string) {
        let parseData : UserDataType = JSON.parse(data);

        let userComp = socektEnv.UpdateUserLoginInfo(parseData.socket_id, parseData.user_name, parseData.user_id, parseData.room_id, parseData.userType);

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