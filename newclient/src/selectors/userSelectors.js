import { createSelector } from 'reselect'
import { getMarketData } from './marketDataSelectors';

export const getUserApiKeys = state => state.user.apiKeys;

export const getExchangesWithApiKey = createSelector([getUserApiKeys, getMarketData], (apiKeys, marketData) => {
  return marketData.map(o => {
    const apiKey = apiKeys.find(p => p.exchangeId === o.id);
    return{
      exchangeName: o.name,
      exchangeId: o.id,
      apiKey: apiKey ? apiKey.apiKey : null,
      apiKeyId: apiKey ? apiKey.id : null,
    }
  });
})