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

    UpdateUserLoginInfo(socketID : string, name : string, student_id : string, room_id : string, type : UserStatus, mobile_phone? : number) {

        if (this.users.has(socketID)) {
            let userComp = this.users.get(socketID);

            if (name) userComp.name = name;

            if (student_id) userComp.user_id = student_id;

            if (room_id) userComp.room_id = room_id;
            
            if (mobile_phone) userComp.mobilephone = mobile_phone;

            userComp.type = type;

            this.users.set(socketID, userComp);
        }

        //Update UserSocketTable
        this.userSocketTable.set(student_id, socketID);
    }

    public UserJoin(socketInfo : SocketIO.Socket) {
        let userComp = CreateUserType(socketInfo);
        this.users.set(socketInfo.id, userComp);

        console.log(userComp);
    }

    UserDisconnect(socketID : string) {
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
     * Only For Student Type
     * @memberof SocketEnvironment
     */
    CheckIfRoomAvailable(userComp : UserComponentType) {
        if (userComp.type !=  UserStatus.Student || !userComp.room_id) return;


    }
}

export default SocketEnvironment;
