import {
  GET_ASSETS,
  UPDATE_PAPER_ASSETS,
} from './types';

import {
  assetService
} from '../services';

function getAssets() {
  return dispatch => dispatch({
    type: GET_ASSETS,
    payload: assetService.getAssets()
  }).catch(err => {
    console.error('getPaperAssets');
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
    // dispatch(alertActions.error(err.message));
  })
}

export const assetActions = {
  getAssets,
  updatePaperAssets
}