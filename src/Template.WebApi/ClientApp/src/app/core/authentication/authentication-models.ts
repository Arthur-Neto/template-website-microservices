export class AuthenticateCommand {
    username: string;
    password: string;
}

export enum Role {
    Manager = 1,
    Client = 2
}

export class AuthenticatedUser {
    id: number;
    username: string;
    password: string;
    role: Role;
    token?: string;
}
