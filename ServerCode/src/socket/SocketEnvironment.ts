import {RoomComponentType, UserComponentType, UserStatus} from '../Utility/Flag/TypeFlag';
import {CreateUserType} from '../Utility/SocketUtility';
import UserEmitter from './Listener/UserEmitter';
import { start } from 'repl';

class SocketEnvironment {

    //UserID -> SocketID
    userSocketTable : Map<string, string>;
    socketID2SocketTable : Map<string, SocketIO.Socket>;
    users : Map<string, UserComponentType>;
    rooms : Map<string, RoomComponentType>;
    cacheLastRoomHistory : Map<string, string>;
    userEmitter : UserEmitter;

    constructor(userEmitter : UserEmitter) {
        this.userEmitter = userEmitter;
        this.userSocketTable = new Map<string, string>();
        this.socketID2SocketTable = new Map<string, SocketIO.Socket>();
        this.users = new Map<string, UserComponentType>();
        this.rooms = new Map<string, RoomComponentType>();
        this.cacheLastRoomHistory = new Map<string, string>();
    }

    CreateRoom(host_id : string, room_id: string, socket_id : string) : boolean {
        //No duplicate room
        if (this.rooms.has(room_id))  {
            let existRoom = this.rooms.get(room_id);

            //Someone else, is using the classRoom
            if (existRoom.host_id != host_id)
                return false;
        }

        this.rooms.set(room_id, {
            host_id : host_id,
            room_id : room_id,
            start_time : 0,
            end_time : 0,
            students : []
        });

        this.UpdateUserLoginInfo(socket_id, null, host_id, room_id, UserStatus.Teacher);

        return true;
    }

    UpdateUserLoginInfo(socketID : string, name : string, user_id : string, room_id : string, type : UserStatus, mobile_phone? : number) : UserComponentType {
        let userComp = null;

        if (this.users.has(socketID)) {
            userComp = this.users.get(socketID);

            if (name) userComp.name = name;

            if (user_id) userComp.user_id = user_id;

            if (room_id) userComp.room_id = room_id;
            
            if (mobile_phone) userComp.mobilephone = mobile_phone;

            userComp.type = type;

            this.users.set(socketID, userComp);
        }

        //Update UserSocketTable
        this.userSocketTable.set(user_id, socketID);

        return userComp;
    }

    public UserJoin(socketInfo : SocketIO.Socket) {
        let userComp = CreateUserType(socketInfo);
        this.users.set(socketInfo.id, userComp);
        this.socketID2SocketTable.set(socketInfo.id, socketInfo);
    }

    UserDisconnect(socketID : string) : UserComponentType {
        let userComp = this.users.get(socketID);

        if (userComp == null) return null;
        console.log("UserDisconnect");

        //Remove student from classroom
        this.LeaveRoom(userComp.user_id, userComp.room_id, userComp.type);

        this.userSocketTable.delete(userComp.user_id);
        this.users.delete(socketID);
        this.socketID2SocketTable.delete(socketID);
        return userComp;
    }

    LeaveRoom(user_id : string, room_id : string, type : UserStatus) : boolean {
        if (!this.userSocketTable.has(user_id)) return false;
        console.log("LeaveRoom " + type);
        let socketID = this.userSocketTable.get(user_id);

        if (this.rooms.has(room_id) && room_id) {
            let room = this.rooms.get(room_id);

            //Teacher has leave the room, remove everyone inside
            if (type ==  UserStatus.Teacher && room.host_id == user_id) {

                this.RoomDismiss(room.room_id);
            } else {
                let studentIndex = room.students.indexOf(socketID);
                room.students = room.students.splice(studentIndex,1);
                this.rooms.set(room_id, room);
            }
            return true;
        }

        return false;
    }

    /**
     * Execute by Teacher, stop the game, and inform student where to rally
     *
     * @param {string} room_id
     * @param {string} location_id If not list, use the last location_id as default
     * @memberof SocketEnvironment
     */
    RoomDismiss(room_id : string, location_id = "K") {
        this.userEmitter.EmitForceLeave(room_id, location_id);
        this.rooms.delete(room_id);
    }

    /**
     *
     * Only For Student Type, and has not join any room
     * @memberof SocketEnvironment
     */
    CheckIfRoomAvailable(userComp : UserComponentType) : boolean{

        if (userComp.type !=  UserStatus.Student || !userComp.room_id) return false;

        if (!this.rooms.has(userComp.room_id)) return false;

        let roomComp = this.rooms.get(userComp.room_id);
        let index = roomComp.students.indexOf(userComp.socket_id);

        //Is unique user
        if (index >= 0)  return false

        roomComp.students.push(userComp.socket_id);
        this.rooms.set(userComp.room_id, roomComp);

        return true;
    }

    AutoJoinAllUserInClass(socket : SocketIO.Socket, room_id : string) {
        this.users.forEach(userComp => {
            if (userComp && userComp.room_id == room_id && this.CheckIfRoomAvailable(userComp)) {
                let userSocket = this.socketID2SocketTable.get(userComp.socket_id);

                if (userSocket != null)
                    this.userEmitter.EmitUserJoinRoom(userSocket, room_id, userComp);   
            }
        });
    }

    FindAllUserInClass(room_id : string) : UserComponentType[]{
        let userComps : UserComponentType[] = [];
        this.users.forEach(userComp => {
            if (userComp && userComp.room_id == room_id) {
                userComps.push(userComp);
            }
        });
        return userComps;
    }

    SetRoomTimer(room_id : string, start_time : number) {
        if (this.rooms.has(room_id)) {
            let room = this.rooms.get(room_id);

            let endExtendMinute = 40;
            let endDateMiliSecond = new Date(start_time + endExtendMinute * 60000);
    
            room.start_time = start_time;
            room.end_time = endDateMiliSecond.getTime();

            this.rooms.set(room_id, room);
        }
    }
}

export default SocketEnvironment;
