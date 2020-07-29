import {SocketIOKey, LoginReturnType} from '../../socket/Flag/TypeFlag';
import {GenerateRandomString} from '../../socket/Utility/GeneralMethod';

export default class LoginModel {

    Login(type : string, account: string, password : string) : LoginReturnType {

        // let defaultData : LoginReturnType = {
        //     status : false
        // }

        if (type == SocketIOKey.teacherType) {

        } else if (type == SocketIOKey.studentType) {
            
        }

        return {
            status : false
        };
    }
    
    TeacherLogin(account: string, password : string) : LoginReturnType {
        return {
            status : true,
            username : "FakeTeacherName",
            user_id : GenerateRandomString(8)
        };
    }

    StudentLogin(account: string) : LoginReturnType {
        return {
            status : true,
            username : "FakeStudentName",
            user_id : GenerateRandomString(8)
        };
    }

}