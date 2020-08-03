import Database from '../Database';


export default class ScoreTableModel {
    _database : Database;

    constructor(database : Database) {
        this._database = database;
    }

    async GetStudentScores(student_id : string) {
        let query = `SELECT student_id, mission_id, score 
                    FROM ScoreTable
                    WHERE student_id='${student_id}'
                    ORDER BY score DESC`;
        return await this._database.ExecuteQuery(query);
    }

    
}