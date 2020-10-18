import {RoomComponentType, UserComponentType, UserStatus, ReserveUserType, ReconnectRequestType} from '../Utility/Flag/TypeFlag';
import {CreateUserType} from '../Utility/SocketUtility';
import UserEmitter from './Listener/UserEmitter';
import SocketEnvironment from './SocketEnvironment';
import SocketManager from './SocketManager';

class SocketReconnectHelper {

    fps : number = 1;

    //First Letter(Minutes)
    reserveTime : number = 20 * 60000;
    userEmitter : UserEmitter;
    socketEnv : SocketEnvironment;
    socketManager : SocketManager;

    reserveUserList : ReserveUserType[];
    reconnectIdToOldID : Map<string, string>;

    constructor( socketManager : SocketManager,  userEmitter : UserEmitter, env : SocketEnvironment) {
        this.socketManager = socketManager;
        this.userEmitter = userEmitter;
        this.socketEnv = env;
        this.reserveUserList = [];
        this.reconnectIdToOldID = new Map<string, string>();   
        this.SetUpdateLoop(this.fps);
    }

    SetUpdateLoop(fps : number) {
        let _this = this;
        setInterval(function() {
            _this.Update();
        }, 1000 / fps);
    }

    GetPairSocketID(socketKeyString : string) : string{
        if (this.reconnectIdToOldID.has(socketKeyString)) {
            return this.reconnectIdToOldID.get(socketKeyString);
        }
        return socketKeyString;
    }

    RepairSocketID(reconnectRequestType:ReconnectRequestType, socket : SocketIO.Socket) {
        this.CleanUpSocketID(reconnectRequestType.reconnect_sid, reconnectRequestType.target_sid);
        this.socketEnv.users.delete(socket.id);
        this.socketEnv.socketID2SocketTable.delete(socket.id);

        if (this.socketEnv.socketID2SocketTable.has(reconnectRequestType.target_sid)) {
            this.socketEnv.socketID2SocketTable.set(reconnectRequestType.target_sid, socket);

            this.reconnectIdToOldID.set(reconnectRequestType.reconnect_sid, reconnectRequestType.target_sid);

            let userComp = this.socketEnv.users.get(reconnectRequestType.target_sid);
            if (userComp != null && userComp.room_id != null) {
                socket.join(userComp.room_id);
            }
        }
    }

    TempDisconnectUser(socket : SocketIO.Socket) {
        let ReconnectRequestType : ReserveUserType = {
            socket_id : socket.id,
            root_id : this.GetPairSocketID(socket.id),
            startReserveTime : Date.now()
        }
        
        this.reconnectIdToOldID.delete(socket.id);

        if (!this.socketEnv.users.has(ReconnectRequestType.root_id)) return;

        this.reserveUserList.push(ReconnectRequestType);
    }

    Update() {
        let reserveCount = this.reserveUserList.length;
        let currentTime = Date.now();
        try {
            for (let i = 0; i < reserveCount; i++) {
                let reserveComp = this.reserveUserList[i];
                // console.log("currentTime " + currentTime +", waitTime" + (reserveComp.startReserveTime + this.reserveTime));
    
                if (currentTime > (reserveComp.startReserveTime + this.reserveTime)) {
                    let targetSocketID = this.reconnectIdToOldID.get(reserveComp.socket_id);
                    this.CleanUpSocketID(reserveComp.socket_id, reserveComp.root_id);
                    this.socketManager.Disonnect(reserveComp.root_id);
                }
            }
        } catch {

        }
    }

    CleanUpSocketID(socket_id : string, root_id : string) {
        this.reconnectIdToOldID.delete(socket_id);

        let reserveIndex = this.reserveUserList.findIndex(x=>x.socket_id == root_id);
        if (reserveIndex >= 0)
            this.reserveUserList.splice(reserveIndex, 1);
    }
}

export default SocketReconnectHelper;