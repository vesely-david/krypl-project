import { createSelector } from 'reselect'
import {
  PAPER_TESTING,
  REAL,
} from '../common/tradingModes';
import { getCurrencyValues } from './marketDataSelectors';

export const getRawAssets = state => state.assets.assets;

export const getAllActiveAssets = createSelector([getRawAssets], (rawAssets) => {
  return rawAssets.filter(o => o.isActive);
})

export const getGroupedAssetsObject = createSelector([getAllActiveAssets], (assets) => {
  return assets.reduce((res, val) => {
    if(!res[val.tradingMode]) res[val.tradingMode] = {};
    if(!res[val.tradingMode][val.exchange]) res[val.tradingMode][val.exchange] = {};
    if(!res[val.tradingMode][val.exchange][val.currency]) res[val.tradingMode][val.exchange][val.currency] 
      = {free: 0, taken: 0, freeAssetId: null, currency: val.currency}
    if(val.strategyId) res[val.tradingMode][val.exchange][val.currency].taken += val.amount;
    else{
      res[val.tradingMode][val.exchange][val.currency].free = val.amount;
      res[val.tradingMode][val.exchange][val.currency].freeAssetId = val.id;
    }
    return res;
  }, {})
})

const exchnageFlatTransformation = tradingModeAssets => {
  if(!tradingModeAssets) return [];
  const exchanges = Object.keys(tradingModeAssets);
  return exchanges.reduce((res, val) => {
    var currencies = Object.keys(tradingModeAssets[val]);
    var currencyArray = currencies.map(o => tradingModeAssets[val][o]);
    res.push({exchange: val, assets: currencyArray})
    return res;
  }, [])
}

export const getGroupedAssets = createSelector([getGroupedAssetsObject], (assets) => {
  const result =  {
    groupedPaperAssets: exchnageFlatTransformation(assets[PAPER_TESTING]),
    groupedRealAssets: exchnageFlatTransformation(assets[REAL]),
  }
  return result; 
})

const mapAsset = asset => ({
  id: asset.id, 
  currency: asset.currency, 
  amount: asset.amount, 
  exchange: asset.exchange,
  strategyId: asset.strategyId,
})

export const getAssets = createSelector([getAllActiveAssets], (assets) => {
  const result =  assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      res.paperAssets.push(mapAsset(val));
    } else if(val.tradingMode === REAL){
      res.realAssets.push(mapAsset(val));
    }
    return res;
  }, {paperAssets: [], realAssets: []})
  return result;
})

export const getAllAssets = createSelector([getRawAssets], (assets) => {
  const result =  assets.map(val => {
    // if(val.tradingMode === PAPER_TESTING){
      return {...val}
    // }
  })
  return result;
})


export const getEvaluatedAssets = createSelector([getAllActiveAssets, getCurrencyValues], (assets, currentValues) => {
  return assets.map(o => {
    if(!currentValues[o.exchange] || !currentValues[o.exchange][o.currency]){
      return{
        ...o,
        btcValue: 0,
        usdValue: 0,
      } 
    }
    return {
      ...o,
      btcValue: currentValues[o.exchange][o.currency].btcValue * o.amount,
      usdValue: currentValues[o.exchange][o.currency].usdValue * o.amount,
    }
  })
})
