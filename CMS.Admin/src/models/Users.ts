
export interface Users {
    PasswordSalt: string;
    UserId: number;
    UserName: string;
    Active: boolean;
    Email: string;
    Password: string;
    CreatedTime: Date;
    LastLogin: Date;
    Lang: string;
}
