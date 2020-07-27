const http = require('http');
const Koa = require('koa');
const bodyParser = require('koa-bodyparser');
const static = require('koa-static')
const path = require('path')
const rootRouter = require('./routing');

const Router = require('koa-router');
const router = new Router();
const views = require('koa-views');
const env = process.env;

const app = new Koa();

let rootFolder : string = path.join(__dirname, '..',);

app.use(static(
  path.join( rootFolder,  '/views')
));

console.log("fff");

app.use(views(rootFolder + '/views', {
  map: {
    html: 'handlebars'
  }
}));

app.use(bodyParser());
app.use(router.routes())
app.use(router.allowedMethods())

var server = http.Server(app.callback());

rootRouter(router, rootFolder);

//"192.168.0.86"
server.listen(env.NODE_PORT || 8020, 'localhost', function () {
  console.log(`Application worker ${process.pid} started...`);
});
