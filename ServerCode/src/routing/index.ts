
import * as path from 'path';
import * as Router from 'koa-router';
import Models from '../service/Models';

module.exports =  (router : Router, rootPath:string, model : Models) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

  router.post('/login', async function (ctx:any, next:any) {    
    ctx.body = await model.UserModel.Login(ctx.request.body['type'], ctx.request.body['account'], 
                                            ctx.request.body['password']);
  });

  //#region Teacher Http Request
  router.get('/getAllStudentByID/:class_id/:year', async function (ctx:any, next:any) {
    ctx.body = await model.UserModel.GetAllStudentInClass(ctx.params.class_id, ctx.params.year);
  });

  router.get('/getAllClassInfo/:from_year', async function (ctx:any, next:any) {
    ctx.body = await model.ClassModel.GetAllAvailableClass(ctx.params.from_year);
  });
//#endregion


}
