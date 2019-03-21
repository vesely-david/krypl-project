import{
  LOGIN,
  LOGOUT
} from './types';

import {
  userService
} from '../services';

import { marketDataActions } from './marketDataActions';
import { assetActions } from './marketDataActions';

function login(credentials) {
  return dispatch => dispatch({
    type: LOGIN,
    async payload() {
      var response = await userService.login(credentials);
      localStorage.setItem('token', response.jwt)
      dispatch(marketDataActions.getMarketData());
      dispatch(assetActions.getAssets());
      dispatch(marketDataActions.getCurrencyValues());
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