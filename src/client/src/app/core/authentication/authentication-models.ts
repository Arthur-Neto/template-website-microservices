export interface IAuthenticateCommand {
    username: string;
    password: string;
}

export enum Role {
    Manager = 1,
    Client = 2
}

export interface IAuthenticatedUser {
    id: number;
    username: string;
    password: string;
    role: Role;
    token?: string;
}
