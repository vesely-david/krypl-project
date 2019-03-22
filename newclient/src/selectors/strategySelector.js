import { createSelector } from 'reselect'
import { getAllAssets } from './assetSelectors';
import { getCurrencyValues } from './marketDataSelectors';

export const getRawPaperStrategies = state => state.paper.strategies;

export const getStrategyCurrentValues = createSelector([getAllAssets, getCurrencyValues], (assets, currentValues) => {
  return assets.reduce((res, val) => {
    if(!val.strategyId || !currentValues[val.exchange] || !currentValues[val.exchange][val.currency]) return res;
    if(!res[val.strategyId]) res[val.strategyId] = {timestamp: Date.now(), usdValue: 0, btcValue: 0};
    res[val.strategyId].btcValue += currentValues[val.exchange][val.currency].btcValue * val.amount;
    res[val.strategyId].usdValue += currentValues[val.exchange][val.currency].usdValue * val.amount;
    return res;
  }, {})
});

export const getPaperStrategies = createSelector([getRawPaperStrategies, getStrategyCurrentValues], (strategies, strategyValues) => {
  return strategies.map(o => 
    ({...o, currentValue: strategyValues[o.id] ? strategyValues[o.id] : {timestamp: Date.now(), usdValue: 0, btcValue: 0}}));
})

export const getAllStrategies = createSelector([getPaperStrategies], (papeStrategies) => {
  let paper = papeStrategies.reduce((res, val) => {
    res[val.id] = val;
    return res;
  }, {})

  // let real = realStrategies.reduce((res, val) => {

  // }, paper);
  return paper;
})