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
    connection : boolean,
    
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
    room_id : string,
    root_socket_id : string
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
    result? : any
}

export interface UserDataType {
    userType : UserStatus,
    socket_id : string,
    user_id : string,
    user_name : string,
    room_id? : string
}

export interface RoomStudentType {
    socketID : string,
    user_id : string
}

export interface TerminateEventType {
    location_id: string,
    room_id : string
}

export interface ReserveUserType {
    socket_id : string,
    root_id : string,
    startReserveTime : number
}

export interface AccessTokenType {
    socket_id : string
}

export interface ReconnectRequestType
{
    reconnect_sid: string;
    target_sid : string;
}

export const ClassCSVKey = {
    id : "序號",
    year : "學年",
    semester : "學期",
    class_id : "班級代碼",
    class_name : "班級名稱",
    grade : "年級"
}