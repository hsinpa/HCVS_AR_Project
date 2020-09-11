import {SocketIOKey, LoginReturnType, UserStatus, DatabaseResultType} from '../../Utility/Flag/TypeFlag';
import {GenerateRandomString} from '../../Utility/GeneralMethod';
import Database from '../Database';
import ClassModel from './ClassModel';

export default class LoginModel {
    _database : Database;
    _classModel : ClassModel;

    constructor(database : Database) {
        this._database = database;
    }
    
    AppendModels(classModel : ClassModel) {
        this._classModel = classModel;
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
    
    async TeacherLogin(account: string, password : string) : Promise<DatabaseResultType> {

        let q = `SELECT id, account_name
                FROM Teacher 
                WHERE id=? AND password=?`;

        let r = await this._database.PrepareAndExecuteQuery(q, [account, password]);
        let s : DatabaseResultType= {
            status : false,
            result : {}
        };

        console.log(r.result);
        r.result = JSON.parse(r.result);

        if (r.result.length > 0) {
            s.status = true;
            s.result = JSON.stringify({user_id : r.result[0]['id'], username : r.result[0]['account_name']});
        }

        return s;
    }

    async StudentLogin(account: string) {
        let q = `SELECT id, student_name, class_id 
                FROM Student 
                WHERE id=?`;

        let r = await this._database.PrepareAndExecuteQuery(q, [account]);
        let s : DatabaseResultType= {
            status : false,
            result : {}
        };
        
        r.result = JSON.parse(r.result);

        if (r.result.length > 0) {
            s.status = true;
            s.result = JSON.stringify({
                user_id : r.result[0]['id'],
                username : r.result[0]['student_name'],
                seat : r.result[0]['seat'],
                room_id : r.result[0]['class_id'] 
            });
        }
        
        return s;
    }

    async Register(account: string, name : string, class_id : string) {
        let isAccountValid = await this.ValidUserAccount(account);
        let isClassValid = await this._classModel.IsClassIDExist(class_id);

        if (isAccountValid && isClassValid) {
            let query = `INSERT INTO Student(id, student_name, class_id)
                        VALUES(?,?,?)`;

           return await(this._database.PrepareAndExecuteQuery(query, [account, name, class_id]));
        }

        return {
            status : false
        };
    }

    async ValidUserAccount(account: string) {
        let query = `SELECT COUNT(*) as count 
                    FROM Student
                    WHERE id =?`;

        let r = await(this._database.PrepareAndExecuteQuery(query, [account]));
        return JSON.parse(r.result)[0]['count'] <= 0;
    }

    async GetAllStudentInClass(classroom_id:string, year : number) {
        let query = `SELECT id, student_name, class_id 
                    FROM Student
                    WHERE class_id =?`;

        return await(this._database.PrepareAndExecuteQuery(query, [classroom_id]));
    }

    async ResetTable() {
        let query = `TRUNCATE TABLE Student`;

        return await(this._database.PrepareAndExecuteQuery(query));
    }

}