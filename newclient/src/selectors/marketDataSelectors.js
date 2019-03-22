import { createSelector } from 'reselect'

export const getMarketData = state => state.marketData.marketData;

export const getCurrencyValuesRaw = state => state.marketData.currencyValues;

export const getCurrencyValues = createSelector([getCurrencyValuesRaw], (values) => {
  const exchanges = Object.keys(values);
  return exchanges.reduce((res, val) => {
    res[val] = values[val].reduce((r,v) => {
      r[v.currency] = {
        btcValue: v.btcValue,
        usdValue: v.usdValue,
      }
      return r;
    }, {});
    return res;
  },{})
})