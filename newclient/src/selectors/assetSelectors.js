import { createSelector } from 'reselect'
import {
  PAPER_TESTING
} from '../common/tradingModes';

export const getRawAssets = state => state.assets.assets;

export const getGroupedAssetsObject = createSelector([getRawAssets], (assets) => {
  const result =  assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      if(!res.groupedPaperAssets[val.exchange]) res.groupedPaperAssets[val.exchange] = {};
      if(!res.groupedPaperAssets[val.exchange][val.currency]) res.groupedPaperAssets[val.exchange][val.currency] = {free: 0, taken: 0, freeAssetId: null}
      if(val.strategyId) res.groupedPaperAssets[val.exchange][val.currency].taken += val.amount;
      else{
        res.groupedPaperAssets[val.exchange][val.currency].free = val.amount;
        res.groupedPaperAssets[val.exchange][val.currency].freeAssetId = val.id;
      } 
      return res;
    } else return res;
  }, {groupedPaperAssets: {}})
  return result;
})

export const getGroupedAssets = createSelector([getGroupedAssetsObject], (assets) => {
  const paperExchanges = Object.keys(assets.groupedPaperAssets);
  const paperGroup = paperExchanges.reduce((res, val) => {
    if(!res[val]) res[val] = [];
    const currencies = Object.keys(assets.groupedPaperAssets[val]);
    currencies.forEach(p => {
      res[val].push({
        currency: p,
        free: assets.groupedPaperAssets[val][p].free,
        taken: assets.groupedPaperAssets[val][p].taken,
        freeAssetId: assets.groupedPaperAssets[val][p].freeAssetId,
      })
    })
    return res;
  }, {})
  return {
    groupedPaperAssets: paperGroup,
  };
})

export const getAssets = createSelector([getRawAssets], (assets) => {
  const result =  assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      if(!res.paperAssets) res.paperAssets = [];
      res.paperAssets.push({
        id: val.id, 
        currency: val.currency, 
        amount: val.amount, 
        exchange: val.exchange,
        strategyId: val.strategyId,
      });
      return res;
    } else return res;
  }, {paperAssets: []})
  return result;
})

export const getAllAssets = createSelector([getRawAssets], (assets) => {
  const result =  assets.map(val => {
    // if(val.tradingMode === PAPER_TESTING){
      return {
        id: val.id, 
        currency: val.currency, 
        amount: val.amount, 
        exchange: val.exchange,
        strategyId: val.strategyId,
      }
    // }
  })
  return result;
})
