import{
  LOGIN,
  REGISTER,
  LOGOUT,
  GET_API_KEYS,
  EDIT_API_KEY,
  DELETE_API_KEY,
} from './types';

import {
  userService
} from '../services';

import { marketDataActions } from './marketDataActions';
import { assetActions } from './assetActions';

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

function register(credentials) {
  return dispatch => dispatch({
    type: REGISTER,
    payload: userService.register(credentials)
  }).catch(err => {
    // dispatch(alertActions.error(err.message));
  })
}

function getApiKeys() {
  return dispatch => dispatch({
    type: GET_API_KEYS,
    payload: userService.getApiKeys()
  }).catch(err => {
    // dispatch(alertActions.error(err.message));
  })
}

function deleteApiKey(apiKeyId){
  return dispatch => dispatch({
    type: DELETE_API_KEY,
    payload: {
      promise: async () => {
        await userService.deleteApiKey(apiKeyId);
        dispatch(getApiKeys())
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err.message));
  })
}

function editApiKey(apiKey){
  return dispatch => dispatch({
    type: EDIT_API_KEY,
    payload: {
      promise: async () => {
        await userService.editApiKey(apiKey);
        dispatch(getApiKeys())
      }
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
  register,
  logout,
  getApiKeys,
  deleteApiKey,
  editApiKey,
}