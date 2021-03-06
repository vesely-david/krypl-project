import { createSelector } from 'reselect'
import { getEvaluatedAssets } from './assetSelectors';
import {
  PAPER_TESTING
} from '../common/tradingModes';

export const getRawPaperOverview = state => state.paper.overview;
export const getRawRealOverview = state => state.real.overview;
export const getRawBackOverview = state => state.back.overview;


export const getCurrentOverviewValues = createSelector([getEvaluatedAssets], (assets) => {
  return assets.reduce((res, val) => {
    if(val.tradingMode === PAPER_TESTING){
      res.paper.btcValue += val.btcValue;
      res.paper.usdValue += val.usdValue;
      if(val.strategyId){
        res.paperReserved.btcValue += val.btcValue;
        res.paperReserved.usdValue += val.usdValue;
      }
    }
    // if(val.tradingMode === BACK_TESTING)
    return res;
  }, {
    paper: {btcValue: 0, usdValue: 0},
    paperReserved: {btcValue: 0, usdValue: 0},
    real: {btcValue: 0, usdValue: 0},
    realReserved: {btcValue: 0, usdValue: 0},
    backtest: {btcValue: 0, usdValue: 0},
    backtestReserved: {btcValue: 0, usdValue: 0},
  })
})

export const getPaperOverview = createSelector([getRawPaperOverview, getCurrentOverviewValues], (overview, currentValues) => {
  return {...overview, currentValue: currentValues.paper, reserved: currentValues.paperReserved};
})

export const getRealOverview = createSelector([getRawRealOverview, getCurrentOverviewValues], (overview, currentValues) => {
  return {...overview, currentValue: currentValues.real, reserved: currentValues.realReserved};
})

