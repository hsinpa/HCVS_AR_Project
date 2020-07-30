import Database from '../Database';


export default class ClassModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    async GetAllAvailableClass(from_year : number) {
        let query = `SELECT class_name, class_id, year 
                    FROM ClassRoom
                    WHERE year >= ${from_year}`;
        return await this._database.ExecuteQuery(query);
    }
}