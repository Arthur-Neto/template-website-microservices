export interface IUserCreateCommand {
    username: string;
    password: string;
}

export interface IUserUpdateCommand {
    id: number;
    username: string;
    password: string;
}
