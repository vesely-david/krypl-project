import { createSelector } from 'reselect'
import { getEvaluatedAssets } from './assetSelectors';

export const getRawPaperStrategies = state => state.paper.strategies;


export const getStrategyCurrentValues = createSelector([getEvaluatedAssets], (assets) => {
  return assets.reduce((res, val) => {
    if(!val.strategyId) return res;
    if(!res[val.strategyId]) res[val.strategyId] = {timestamp: Date.now(), usdValue: 0, btcValue: 0};
    res[val.strategyId].btcValue += val.btcValue;
    res[val.strategyId].usdValue += val.usdValue;
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