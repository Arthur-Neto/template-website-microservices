export class UserCreateCommand {
    username: string;
    password: string;
}

export class UserUpdateCommand {
    id: number;
    username: string;
    password: string;
}
