var axios = require('axios');
var program = require('commander');

// const marketUrl = 'http://localhost:9999';
const marketUrl = 'https://marketData.jankirchner.cz';
const masterUrl = 'https://api.jankirchner.cz';
// const masterUrl = 'http://localhost:54850';
const exchange = 'poloniex';

function getRandom(min, max) {
  return Math.random() * (max - min) + min;
}

const getRates = async () => {
  const result = await axios.get(`${marketUrl}/business/rate/${exchange}`);
  return result.data;
}

const getAssets = async (strategyId) => {
  const result = await axios.get(`${masterUrl}/assets/${strategyId}`);
  return result.data;
}

const getTrades = async (strategyId) => {
  const result = await axios.get(`${masterUrl}/assets/${strategyId}`);
  return result.data;
}

const cancelTrade = async (tradeId) => {
  await axios.delete(`${masterUrl}/trades/${tradeId}`);
}

const trade = async (strategyId) => {
  const assets = await getAssets(strategyId);
  const freeAssets = assets.filter(o => !o.isReserved);
  const item = freeAssets[Math.floor(Math.random() * freeAssets.length)];
  if(!item){
    const trades = await getTrades(strategyId);
    const openedTrades = trades.filter(o => !o.closed)
    var tradeToClose = openedTrades[Math.floor(Math.random() * openedTrades.length)];
    try{
      await cancelTrade(tradeToClose.id);
      console.log(`Trade canceled: ${tradeToClose.id}`);
    } catch(e){
      console.error(`Trade NOT canceled: ${tradeToClose.id}`);
    }
    return;
  };

  const rates = await getRates();
  const markets = rates.filter(o => o.symbol.startsWith(item.currency + '_' ) || o.symbol.endsWith( '_' + item.currency))
  const market = markets[Math.floor(Math.random() * markets.length)];

  const obj = {
    exchange: 'poloniex',
    symbol: market.symbol,
    amount: Math.random() > 0.75 ? getRandom(item.amount / 2, item.amount) : item.amount,
  };
  try{
    var res = await axios.post(`${masterUrl}/trade/${strategyId}/${market.symbol.startsWith(item.currency + '_') ? 'buy' : 'sell'}`, 
    JSON.stringify(obj), {headers: {'Content-Type' : 'application/json'}})
    console.log(`New trade: ${res.data}`);
    console.log({...obj, type: market.symbol.startsWith(item.currency + '_') ? 'buy' : 'sell'});
  } catch(e){
    console.error(`Trade NOT created: ${obj}`);
    
  }
}

const main = async (strategyId, tradingFrequency) => {
  trade(strategyId);
  setInterval(() => trade(strategyId), tradingFrequency * 60000 );
}

program
  .option('--strategy-id <type>', 'Strategy id')
  .option('--trading-frequency <type>', 'How often should strategy perform trade. In minutes')
  .parse(process.argv);

if(!program.strategyId){
  console.log('--strategy-id must be present');
  return;
}
const tradingFrequencyArg = Number.parseInt(program.tradingFrequency);
if(isNaN(tradingFrequencyArg) || tradingFrequencyArg < 5){
  console.log('--trading-frequency must be present & number & > 5');
  return;
}

main(program.strategyId, tradingFrequencyArg);

// main('d2bf929c-5919-4e4c-9e97-29ece138f397', 5);

