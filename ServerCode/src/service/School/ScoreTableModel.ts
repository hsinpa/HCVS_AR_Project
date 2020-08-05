import Database from '../Database';
import {DatabaseResultType} from '../../Utility/Flag/TypeFlag';

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

    async GetPerticipantCount(class_id : string) {
        let query = `
        SELECT COUNT(student_id) as main_value, mission_id
        FROM ScoreTable
        LEFT JOIN Student
        ON student_id = Student.id
        WHERE Student.class_id = '${class_id}'
        GROUP BY mission_id`;

        return await this._database.ExecuteQuery(query);
    }

    async GetClassAverageScore(class_id : string) {
        let query = `
        SELECT AVG(score) as main_value, mission_id
        FROM ScoreTable
        LEFT JOIN Student
        ON student_id = Student.id
        WHERE Student.class_id = '${class_id}'
        GROUP BY mission_id`;

        return await this._database.ExecuteQuery(query);
    }

    async GetClassScoreInfo(class_id : string) {
        let data : DatabaseResultType = {status : false};

        let participant_count = await this.GetPerticipantCount(class_id);
        let average_score = await this.GetClassAverageScore(class_id);

        if (participant_count.status && average_score.status) {
            data.status = true;
            data.result = {
                participant_count : participant_count.result,
                average_score : average_score.result
            }

            return data;
        }

        return data;
    }

}