import { Users } from "./Users";


export interface Leave_Request {
    Id: number;
    User_LeaveID: number;
    User_ApproveID: number;
    TimeStart: Date;
    TimeEnd: Date;
    Reason: string;
    User_Leave?: Users;
    User_Approve?: Users;
}
