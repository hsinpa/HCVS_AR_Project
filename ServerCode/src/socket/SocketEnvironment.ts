import {RoomComponentType, UserComponentType, UserStatus} from '../Utility/Flag/TypeFlag';
import {CreateUserType} from '../Utility/SocketUtility';

class SocketEnvironment {

    //UserID -> SocketID
    userSocketTable : Map<string, string>;
    users : Map<string, UserComponentType>;
    rooms : Map<string, RoomComponentType>;

    constructor() {
        this.userSocketTable = new Map<string, string>();
        this.users = new Map<string, UserComponentType>();
        this.rooms = new Map<string, RoomComponentType>();
    }

    CreateRoom(host_id : string, room_id: string) : boolean {
        //No duplicate room
        if (this.rooms.has(room_id)) return false;

        this.rooms.set(room_id, {
            host_id : host_id,
            room_id : room_id,
            students : []
        });

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

        console.log(userComp);
    }

    UserDisconnect(socketID : string) : UserComponentType {
        let userComp = this.users.get(socketID);

        //Remove student from classroom
        if (this.rooms.has(userComp.room_id) && userComp.room_id) {
            let room = this.rooms.get(userComp.room_id);
            let studentIndex = room.students.indexOf(socketID);
            room.students = room.students.splice(studentIndex,1);
            this.rooms.set(userComp.room_id, room);
        }

        this.userSocketTable.delete(userComp.user_id);
        this.users.delete(socketID);
        return userComp;
    }

    /**
     * Execute by Teacher
     *
     * @param {string} room_id
     * @param {string} socket_id
     * @memberof SocketEnvironment
     */
    RoomDismiss(room_id : string, socket_id : string) {

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

        if (index >= 0)  return false

        //Is unique user
        roomComp.students.push(userComp.socket_id);
        this.rooms.set(userComp.room_id, roomComp);

        return true;
    }
}

export default SocketEnvironment;
