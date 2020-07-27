
const pathModule = require('path');

module.exports =  (router:any, rootPath:string) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });
}
