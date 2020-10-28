import Database from '../Database';
import UserModel from './UserModel';
import ScoreTableModel from './ScoreTableModel';
import {ClassCSVKey} from '../../Utility/Flag/TypeFlag';

import {parse} from 'csv';

export default class ClassModel {
    _database : Database;
    _userModel : UserModel;
    _scoreTableModel : ScoreTableModel;

    constructor(database : Database) {
        this._database = database;
    }

    AppendModels(userModel : UserModel, scoreTableModel : ScoreTableModel) {
        this._userModel = userModel;
        this._scoreTableModel = scoreTableModel;
    }

    async GetAllAvailableClass() {
        let query = `SELECT id, class_name, class_id, year, grade 
                    FROM ClassRoom
                    ORDER BY year DESC Limit 80`;

        return (await this._database.PrepareAndExecuteQuery(query));
    }

    async IsClassIDExist(class_id: string) {
        let query = `SELECT COUNT(*) as count 
                    FROM ClassRoom
                    WHERE class_id = ?`;

        let r = await(this._database.PrepareAndExecuteQuery(query, [class_id]));
        return JSON.parse(r.result)[0]['count'] == 1;
    }

    async ParseClassInfoCsv(raw_csv_string : string) {        
        //Reset near everything, except Teacher's Info
        await this.ResetTable();
        await this._scoreTableModel.ResetTable();
        await this._userModel.ResetTable();

        let insertStringArray = "";

        parse(raw_csv_string,{ columns: true }).on('data', (row) => {
            //Order as
            //id, year, semester, grade, class_id, class_name
            insertStringArray += `('${row[ClassCSVKey.id]}', ${row[ClassCSVKey.year]}, ${row[ClassCSVKey.semester]}, 
                                    ${row[ClassCSVKey.grade]}, '${row[ClassCSVKey.class_id]}', N'${row[ClassCSVKey.class_name]}' ),`;

        })
        .on('end', async () => {

            const finalInsertQuery = insertStringArray.slice(0, -1);

            await this.InsertTable(finalInsertQuery);
        });
    }

    async InsertTable(insertStringArray : string) {
        let query = ` INSERT INTO ClassRoom (id, year, semester, grade, class_id, class_name)
        VALUES ${insertStringArray};`;
        
        return await(this._database.PrepareAndExecuteQuery(query));
    }

    async ResetTable() {
        let query = `TRUNCATE TABLE ClassRoom`;

        return await(this._database.PrepareAndExecuteQuery(query));
    }


}