
import * as path from 'path';
import * as Router from 'koa-router';
import Models from '../service/Models';
import SocketManager from '../socket/SocketManager';


module.exports =  (router : Router, rootPath:string, model : Models, socketManager : SocketManager) => {
  router.get('/', async function (ctx:any, next:any) {
    ctx.state = {
      title: 'HSINPA'
    };
    await ctx.render('index', {title: "HSINPA"});
  });

  router.get('/upload_info_parser', async function (ctx:any, next:any) {

    await ctx.render('class_info_parser');
  });

  router.get('/manual_leave/:root_id', async function (ctx:any, next:any) {
    console.log("Hello leaver");
    socketManager.DiconnectAndCleanUp(ctx.params.root_id);
    ctx.body = "";
  });

  //#region Teacher Http Request
  router.get('/getAllStudentByID/:class_id/:year', async function (ctx:any, next:any) {
    ctx.body = await model.UserModel.GetAllStudentInClass(ctx.params.class_id, ctx.params.year);
  });

  router.get('/getAllClassInfo', async function (ctx:any, next:any) {
    ctx.body = await model.ClassModel.GetAllAvailableClass();
  });

  router.get('/getClassScoreInfo/:class_id', async function (ctx:any, next:any) {
    ctx.body = await model.ScoreTableModel.GetClassScoreInfo(ctx.params.class_id);
  });

//#endregion

//#region Overall
router.get('/getStudentScore/:student_id', async function (ctx:any, next:any) {
  ctx.body = await model.ScoreTableModel.GetStudentScores(ctx.params.student_id);
});

router.get('/getStudentRank/:class_id', async function (ctx:any, next:any) {
  ctx.body = await model.ScoreTableModel.GetClassRankingScore(ctx.params.class_id);
});

router.post('/upload_class_info', async function (ctx:any, next:any) {
  await model.ClassModel.ParseClassInfoCsv(ctx.request.body.csvfile);

  ctx.body = "";
});

router.post('/insert_student_record', async function (ctx:any, next:any) {
  ctx.body = await model.ScoreTableModel.InsertStudentScore(
    ctx.request.body['student_id'], ctx.request.body['mission_id'], ctx.request.body['score']
  );
});


//#endregion

//#region User Relate 

router.post('/login', async function (ctx:any, next:any) {
  ctx.body = await model.UserModel.Login(ctx.request.body['type'], ctx.request.body['account'], ctx.request.body['password']);
});

router.post('/register', async function (ctx:any, next:any) {
  ctx.body = await model.UserModel.Register(ctx.request.body['account'], ctx.request.body['name'], ctx.request.body['class_id']);
});
//#endregion
}
