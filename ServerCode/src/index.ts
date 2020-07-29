import * as http from 'http';
import * as koa_static from 'koa-static';
import * as Router from 'koa-router';

import SocketIOManager from './socket/SocketManager';
import Models from './service/Models';

const Koa = require('koa');
const bodyParser = require('koa-bodyparser');
const path = require('path')
const sql = require('mssql');
const rootRouter = require('./routing');
const so = require('koa-views');

const router = new Router();
const views = require('koa-views');
const models = new Models();

const dotenv = require('dotenv');
dotenv.config();

const env = process.env;

const app = new Koa();
let rootFolder : string = path.join(__dirname, '..',);

app.use(koa_static(
  path.join( rootFolder,  '/views')
));

app.use(views(rootFolder + '/views', {
  map: {
    html: 'handlebars'
  }
}));

app.use(bodyParser());
app.use(router.routes())
app.use(router.allowedMethods())

rootRouter(router, rootFolder, models);

const config = {
  user : env.DATABASE_USER,
  password : env.DATABASE_PASSWORD,
  database : env.DATABASE_NAME,
  server : env.DATABASE_SERVER,
  options : {
    enableArithAbort : true
  }
}

// const run = async() => {
//   let pool;
//   try {
//     pool = await sql.connect(config);
//     const { recordset } = await sql.query `select * from users;`;

//   } catch(err) {
//   } finally {
//     await pool.close();
//     console.log("Connection Closed");
//   }
// }

// run();

// @ts-ignore
var server = http.Server(app.callback());
var socketServer = new SocketIOManager(server);

//"192.168.0.86"
server.listen(env.NODE_PORT || 8020, 'localhost', function () {
  console.log(`Application worker ${process.pid} started...`);
});
