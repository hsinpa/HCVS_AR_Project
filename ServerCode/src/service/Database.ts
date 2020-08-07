const sql = require('mssql');

import {createConnection} from 'mysql2/promise';

import {DatabaseResultType} from '../Utility/Flag/TypeFlag';
import { ppid } from 'process';

export default class Database {
    config = {};

    constructor(envFile : NodeJS.ProcessEnv) {
        this.config = {
            user : envFile.DATABASE_USER,
            password : envFile.DATABASE_PASSWORD,
            database : envFile.DATABASE_NAME,
            host : envFile.DATABASE_SERVER
        }
    }

    async PrepareAndExecuteQuery(p_query : string, p_params : any[] = []) {
        let pool;
        let dataResult : DatabaseResultType = {
            status : false,
            result : {}
        };

        try {
            pool = await createConnection(this.config);
            let [rows, fields] = await pool.execute(p_query, p_params);

            dataResult.result = JSON.stringify(rows);
            dataResult.status = true;

        } catch (err) {
            dataResult.result = err;
        } finally {
            if (pool != null)
                await pool.end();

            return dataResult;
        }
    }
}