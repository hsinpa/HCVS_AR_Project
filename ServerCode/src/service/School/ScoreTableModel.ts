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
                    WHERE student_id=?
                    ORDER BY score DESC`;
        return await this._database.PrepareAndExecuteQuery(query, [student_id]);
    }

    async GetPerticipantCount(class_id : string) {
        let query = `
        SELECT COUNT(student_id) as main_value, mission_id
        FROM ScoreTable
        LEFT JOIN Student
        ON student_id = Student.id
        WHERE Student.class_id = ?
        GROUP BY mission_id`;

        return await this._database.PrepareAndExecuteQuery(query, [class_id]);
    }

    async GetClassAverageScore(class_id : string) {
        let query = `
        SELECT AVG(score) as main_value, mission_id
        FROM ScoreTable
        LEFT JOIN Student
        ON student_id = Student.id
        WHERE Student.class_id = ?
        GROUP BY mission_id`;

        return await this._database.PrepareAndExecuteQuery(query, [class_id]);
    }

    async GetClassRankingScore(class_id : string) {
        let query = `
        SELECT SUM(IFNULL(score,0)) as total_score, Student.id as student_id, student_name
        FROM Student
        LEFT JOIN ScoreTable
        ON ScoreTable.student_id = Student.id
        WHERE Student.class_id = ?
        GROUP BY Student.id
        ORDER By total_score DESC`;

        return await this._database.PrepareAndExecuteQuery(query, [class_id]);
    }

    async GetClassScoreInfo(class_id : string) {
        let data : DatabaseResultType = {status : false};

        let participant_count = await this.GetPerticipantCount(class_id);
        let average_score = await this.GetClassAverageScore(class_id);

        let participantCountResult = JSON.parse(participant_count.result);
        let averageScoreResult = JSON.parse(average_score.result);

        if (participant_count.status && average_score.status) {
            data.status = true;
            data.result = JSON.stringify({
                participant_count : participantCountResult,
                average_score : averageScoreResult
            });

            return data;
        }

        return data;
    }

    async ResetTable() {
        let query = `TRUNCATE TABLE ScoreTable`;

        return await(this._database.PrepareAndExecuteQuery(query));
    }

}