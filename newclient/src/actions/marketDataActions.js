import {
  GET_MARKET_DATA,
  GET_CURRENCY_VALUES,
} from './types';

import { marketDataService } from '../services/marketDataService';

function getMarketData(){
  return dispatch => dispatch({
    type: GET_MARKET_DATA,
    payload: marketDataService.getMarketData()
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getCurrencyValues(){
  return dispatch => dispatch({
    type: GET_CURRENCY_VALUES,
    payload: marketDataService.getCurrencyValues()
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

export const marketDataActions = {
  getMarketData,
  getCurrencyValues,
}

