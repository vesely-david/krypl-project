import { createSelector } from 'reselect'
import {
  PAPER_TESTING
} from '../common/tradingModes';

export const getRawAssets = state => state.assets.assets;

export const getGroupedAssets = createSelector([getRawAssets], (assets) => {
  const result =  assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      if(!res.groupedPaperAssets[val.exchange]) res.groupedPaperAssets[val.exchange] = [];
      res.groupedPaperAssets[val.exchange].push({id: val.id, currency: val.currency, hold: val.amount, free: val.free});
      return res;
    } else return res;
  }, {groupedPaperAssets: {}})
  return result;
})

export const getAssets = createSelector([getRawAssets], (assets) => {
  const result =  assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      if(!res.paperAssets) res.paperAssets = [];
      res.paperAssets.push({id: val.id, currency: val.currency, amount: val.amount, free: val.free, exchange: val.exchange});
      return res;
    } else return res;
  }, {paperAssets: []})
  return result;
})