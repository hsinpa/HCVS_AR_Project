import EnvironmentComponent from "../Components/EnvironmentComponent";
import {SocketIOKey} from "../Flag/TypeFlag";

 export default class EventProcesser {

    _socketIO : SocketIO.Server;
    _env : EnvironmentComponent;
    _gameLoopFps : number;
    _queueManager : EventQueue;

    constructor(env : EnvironmentComponent, socketIO : SocketIO.Server, gameLoopFps : number) {
        this._socketIO = socketIO;
        this._env = env;

         /**  @type {EventQueue} */
        this._queueManager = new EventQueue(500);
        this.SetGameLoop(gameLoopFps);
    }

    /**
     *
     *
     * @param {number} fps
     */
    SetGameLoop(fps: number) {
        let _this = this;
        setInterval(function() {
            _this.Update();
        }, 1000 / fps);
    }
    
    Update() {
        if (this._queueManager == null)
            return;

        let queueLength = this._queueManager.Count();

        for (let i = 0 ; i < queueLength; i++) {
            let msgJSON = this._queueManager.Dequeue();

            if (msgJSON != null && SocketIOKey.socket_id in msgJSON) {
                if (msgJSON[SocketIOKey.socket_id] in this._env.users) {

                    let user = this._env.users.get(msgJSON[SocketIOKey.socket_id]);

                    //Broadcast to all room member, except sender
                    //if (user != null && user.room_id !== "" &&　user.room_id != null && user.socket != null)
                        // user.socket.to(user.room_id).emit(SocketIOEvent.CastMessage, JSON.stringify( msgJSON));
                }
            }
        }
        this._queueManager.Clear();
    }
}