import {
  GET_MARKET_DATA,
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

export const marketDataActions = {
  getMarketData
}