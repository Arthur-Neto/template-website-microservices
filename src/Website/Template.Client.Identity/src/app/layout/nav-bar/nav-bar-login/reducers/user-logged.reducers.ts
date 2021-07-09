import { createReducer, on } from '@ngrx/store';

import { IAuthenticatedUser } from '@core/authentication/authentication-models';

import { createUserLogged } from '../actions/user-logged.actions';

export const initialState: IAuthenticatedUser = { isAuthenticated: false };

const _userLoggedReducer = createReducer(
    initialState,
    on(createUserLogged, (state, payload) => (state = payload.userLogged))
);

export function userLoggedReducer(state: any, action: any) {
    return _userLoggedReducer(state, action);
}
