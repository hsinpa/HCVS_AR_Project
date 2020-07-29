import {RoomComponentType, UserComponentType} from '../Flag/TypeFlag';

class EnvironmentComponent {
    users : Map<string, UserComponentType>;
    rooms : RoomComponentType[];

    constructor() {
        this.users = new Map<string, UserComponentType>();
        this.rooms = [];
    }
}

export default EnvironmentComponent;
