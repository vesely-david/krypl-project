import {
  GET_PAPER_OVERVIEW,
  GET_PAPER_STRATEGIES,
  REGISTER_PAPER_STRATEGY
} from './types';

import { overviewService } from '../services/overviewService';
import { assetActions } from './assetActions';

function getPaperOverview(){
  return dispatch => dispatch({
    type: GET_PAPER_OVERVIEW,
    payload: overviewService.getPaperOverview()
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function getPaperStrategies(){
  return dispatch => dispatch({
    type: GET_PAPER_STRATEGIES,
    payload: async () =>{
      const res = await overviewService.getPaperStrategies();
      return{
        strategies: res.strategies,
        pagination:{
          page: res.page,
          perPage: res.perPage,
          total: res.count,
        },
      }
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

function registerPaperStrategy(name, exchange, description, assets){
  return dispatch => dispatch({
    type: REGISTER_PAPER_STRATEGY,
    payload: async () => {
      const res = await overviewService.registerPaperStrategy(name, exchange, description, assets);
      dispatch(getPaperOverview());
      dispatch(getPaperStrategies());
      dispatch(assetActions.getAssets());
      return res;
    }
  }).catch(err => {
    // dispatch(alertActions.error(err));
  })
}

export const overviewActions = {
  getPaperOverview,
  getPaperStrategies,
  registerPaperStrategy,
}
