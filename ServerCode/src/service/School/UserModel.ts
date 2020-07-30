import {SocketIOKey, LoginReturnType, UserStatus} from '../../Utility/Flag/TypeFlag';
import {GenerateRandomString} from '../../Utility/GeneralMethod';
import Database from '../Database';

export default class LoginModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    Login(type : UserStatus, account: string, password : string) : LoginReturnType {

        // let defaultData : LoginReturnType = {
        //     status : false
        // }

        if (type == UserStatus.Teacher) {
            return this.TeacherLogin(account, password);
        } else if (type == UserStatus.Student) {
            return this.StudentLogin(account);
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

    async GetAllStudentInClass(classroom_id:string, year : number) {
        let query = `SELECT id, year, semester, student_name, seat, class_id 
                    FROM Student
                    WHERE year = ${year} AND class_id = ${classroom_id}`;
        return await this._database.ExecuteQuery(query);
    }

}