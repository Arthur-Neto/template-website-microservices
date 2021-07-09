import { createAction, props } from '@ngrx/store';

import { IAuthenticatedUser } from '@core/authentication/authentication-models';

export enum ActionTypes {
    New = '[Nav bar] New User Logged',
}

export const createUserLogged = createAction(
    ActionTypes.New,
    props<{ userLogged: IAuthenticatedUser }>()
);
