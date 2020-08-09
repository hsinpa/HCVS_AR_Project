import UserModel from './School/UserModel';
import ClassModel from './School/ClassModel';
import ScoreTableModel from './School/ScoreTableModel';

import Database from './Database';

export default class Models {
    Database : Database;
    UserModel : UserModel; 
    ClassModel : ClassModel;
    ScoreTableModel : ScoreTableModel;

    constructor(envFile : NodeJS.ProcessEnv) {
        this.Database = new Database(envFile);
        this.ClassModel = new ClassModel(this.Database);
        this.UserModel = new UserModel(this.Database);
        this.ScoreTableModel = new ScoreTableModel(this.Database);

        this.ClassModel.AppendModels(this.UserModel, this.ScoreTableModel);
        this.UserModel.AppendModels( this.ClassModel);
    }
}