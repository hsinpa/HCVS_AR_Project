import UserModel from './User/UserModel';

export default class Models {
    UserModel : UserModel; 

    constructor() {
        this.UserModel = new UserModel();
    }
}