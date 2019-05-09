import { createSelector } from 'reselect'
import { getEvaluatedAssets } from './assetSelectors';

export const getRawPaperStrategies = state => state.paper.strategies;
export const getRawRealStrategies = state => state.real.strategies;
export const getRawBacktestStrategies = state => state.back.strategies;


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

export const getRealStrategies = createSelector([getRawRealStrategies, getStrategyCurrentValues], (strategies, strategyValues) => {
  return strategies.map(o => 
    ({...o, currentValue: strategyValues[o.id] ? strategyValues[o.id] : {timestamp: Date.now(), usdValue: 0, btcValue: 0}}));
})

export const getAllStrategies = createSelector([getPaperStrategies, getRealStrategies, getRawBacktestStrategies], (papeStrategies, realStrategies, backtestStrategies) => {
  let paper = papeStrategies.reduce((res, val) => {
    res[val.id] = val;
    return res;
  }, {})
  let real = realStrategies.reduce((res, val) => {
    res[val.id] = val;
    return res;
  }, paper);

  let back = backtestStrategies.reduce((res, val) => {
    res[val.id] = val;
    return res;
  }, real);

  return back;
})