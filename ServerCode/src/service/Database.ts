const sql = require('mssql');
import {DatabaseResultType} from '../Utility/Flag/TypeFlag';

export default class Database {
    config = {};

    constructor(envFile : NodeJS.ProcessEnv) {
        this.config = {
            user : envFile.DATABASE_USER,
            password : envFile.DATABASE_PASSWORD,
            database : envFile.DATABASE_NAME,
            server : envFile.DATABASE_SERVER,
            options : {
              enableArithAbort : true
            }
        }
    }

    async PrepareAndExecuteQuery(p_query : string, p_params : any) {

    }

    async ExecuteQuery(p_query : string) : Promise<DatabaseResultType> {
        let pool;

        let dataResult : DatabaseResultType = {
            status : false,
            result : {}
        };

        try {
            pool = await sql.connect(this.config);
            const { recordset } = await sql.query(p_query);
            dataResult.result = JSON.stringify (recordset);
            dataResult.status = true;
        } catch(err) {

            //Insert Error to result
            dataResult.result = err;
        } finally {
            if (pool != null)
                await pool.close();
            return dataResult;
        }
    }
}