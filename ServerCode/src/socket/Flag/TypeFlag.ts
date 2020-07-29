export enum UserStatus {
   Student, Teacher, Guest
}

export interface EnvironmentType {
    users : Map<string, UserComponentType>
}

export interface UserComponentType {
    socket_id : string,
    socket : SocketIO.Socket,
    name : string,
    student_id : string,

    room_id : string,
    type : UserStatus
}

export interface RoomComponentType {
    room_id : string,
    host_id : string,
    students : UserComponentType[]
}

export interface MessageType {
    event_id : string,
    socket_id : string,
    string_message? : string,
    bool_message? : boolean,
    num_message? : number
}

export const SocketIOKey = {
    socket_id : "socket_id",
    roomCapacity : "room_capacity"
}

export const SocketIOEvent = {
    FindRoom : "event@find_room",
    StartGame : "event@start_game",
    ForceEndGame : "event@force_end_game",
    Rally : "event@rally"
}
