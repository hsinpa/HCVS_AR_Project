import {SocketIOKey, LoginReturnType, UserStatus} from '../../Utility/Flag/TypeFlag';
import {GenerateRandomString} from '../../Utility/GeneralMethod';
import Database from '../Database';

export default class LoginModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    async Login(type : UserStatus, account: string, password : string) {

        if (type == UserStatus.Teacher) {
            return await this.TeacherLogin(account, password);
        } else if (type == UserStatus.Student) {
            return await this.StudentLogin(account);
        }

        return {
            status : false
        };
    }
    
    async TeacherLogin(account: string, password : string) : Promise<LoginReturnType> {
        let q = `SELECT id, account_name, account_type, isValid 
                FROM Teacher 
                WHERE email=${account} AND id=${password}`;

        let r = await this._database.ExecuteQuery(q);
        let s : LoginReturnType= {
            status : false,
        };
        
        if (r.result.length > 0 && r.result[0]['isValid'] == 1) {
            s.status = true;
            s.user_id = r.result[0]['id'];
            s.username = r.result[0]['account_name'];
        }

        return s;
    }

    async StudentLogin(account: string) {
        let q = `SELECT id, student_name, seat, class_id 
                FROM Teacher 
                WHERE email=${account}`;

        let r = await this._database.ExecuteQuery(q);
        let s : LoginReturnType= {
            status : false,
        };

        if (r.result.length > 0) {
            s.status = true;
            s.user_id = r.result[0]['id'];
            s.username = r.result[0]['student_name'];
            s.seat = r.result[0]['seat'];
        }
        
        return s;
    }

    async GetAllStudentInClass(classroom_id:string, year : number) {
        let query = `SELECT id, year, semester, student_name, seat, class_id 
                    FROM Student
                    WHERE year = ${year} AND class_id = ${classroom_id}`;
        return await this._database.ExecuteQuery(query);
    }

}