import {
  GET_ASSETS,
  UPDATE_PAPER_ASSETS,
  UPDATE_REAL_ASSETS,
} from './types';

import {
  assetService
} from '../services';

function getAssets() {
  return dispatch => dispatch({
    type: GET_ASSETS,
    payload: assetService.getAssets()
  }).catch(err => {
    console.error('getAssets');
    // dispatch(alertActions.error(err.message));
  })
}

function updatePaperAssets(assets) {
  return dispatch => dispatch({
    type: UPDATE_PAPER_ASSETS,
    payload: async () => {
      await assetService.updatePaperAssets(assets);
      dispatch(getAssets());
      return true;
    } 
  }).catch(err => {
    console.error('updatePaperAssets');
    return false;
  })
}

function updateRealAssets(exchangeId) {
  return dispatch => dispatch({
    type: UPDATE_REAL_ASSETS,
    payload: async () => {
      await assetService.updateRealAssets(exchangeId);
      dispatch(getAssets());
      return true;
    } 
  }).catch(err => {
    console.error('updateRealAssets');
    return false;
  })
}


export const assetActions = {
  getAssets,
  updatePaperAssets,
  updateRealAssets,
}