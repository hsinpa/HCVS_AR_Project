import Database from '../Database';


export default class ClassModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    async GetAllAvailableClass() {
        let query = `SELECT class_name, class_id, year 
                    FROM ClassRoom
                    ORDER BY year DESC Limit 80`;

        return (await this._database.PrepareAndExecuteQuery(query));
    }
}