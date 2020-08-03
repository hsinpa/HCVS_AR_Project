export enum UserStatus {
   Student, Teacher, Guest
}

export interface EnvironmentType {
    users : Map<string, UserComponentType>
}

export interface UserComponentType {
    socket_id : string
    name : string,
    user_id : string,
    room_id : string,
    type : UserStatus,
    
    //Only teacher might have this value
    mobilephone? : number,
}

export interface RoomComponentType {
    room_id : string,
    host_id : string,
    
    start_time : number,
    end_time : number,

    //SocketID List
    students : string[]
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
    roomCapacity : "room_capacity",

    teacherType : "teacher",
    studentType : "student",
    guestType : "guest",
}

export interface TeacherCreateMsgRoomType {
    user_id : string,
    room_id : string
}

export interface TeacherCommonType {
    room_id : string
}

export interface LoginReturnType { 
    status : boolean,
    username? : string,
    user_id? : string,
    seat? : string,
    room_id? : string
}

export interface DatabaseResultType {
    status : boolean,
    result : any
}

export interface UserDataType {
    userType : UserStatus,
    user_id : string,
    user_name : string,
    room_id? : string
}

export interface RoomStudentType {
    socketID : string,
    user_id : string
}