import{
  LOGIN,
  LOGOUT
} from './types';

import {
  userService
} from '../services';

function login(credentials) {
  return dispatch => dispatch({
    type: LOGIN,
    async payload() {
      var response = await userService.login(credentials);
      localStorage.setItem('token', response.jwt)
      return response;
    }
  }).catch(err => {
    // dispatch(alertActions.error(err.message));
  })
}

function logout() {
  localStorage.removeItem('token');
  return {
    type: LOGOUT,
  };
}

export const userActions = {
  login,
  logout
}