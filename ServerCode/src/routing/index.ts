
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
    
    console.log(ctx.request.body);
    
    ctx.body = {title: "HSINPA"};
  });
}
