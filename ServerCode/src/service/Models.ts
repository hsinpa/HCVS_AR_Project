import UserModel from './School/UserModel';
import ClassModel from './School/ClassModel';

import Database from './Database';

export default class Models {
    Database : Database;
    UserModel : UserModel; 
    ClassModel : ClassModel;

    constructor(envFile : NodeJS.ProcessEnv) {
        this.Database = new Database(envFile);
        this.UserModel = new UserModel(this.Database);
        this.ClassModel = new ClassModel(this.Database);
    }
}