import Database from '../Database';


export default class ClassModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    async GetAllAvailableClass() {
        let query = `SELECT TOP 80 class_name, class_id, year 
                    FROM ClassRoom
                    ORDER BY year DESC`;
        return (await this._database.ExecuteQuery(query));
    }
}