var axios = require('axios');

var marketAPI = 'http://marketData.jankirchner.cz';

const getBinanceCurrencies = async () => {
  var result = await axios.get(`${marketAPI}/exchanges`);
  return result.data;
}

const getPoloniexCurrencies = async () => {
  var result = await axios.get(`https://poloniex.com/public?command=returnCurrencies`);
  return result.data;
}

const getCoinMarketCoins = async () => {
  var result = await axios.get(`https://api.coinmarketcap.com/v2/listings`);
  return result.data;
}

const getInfo = async () => {
  const currencies = await getBinanceCurrencies();
  const poloniexCurrencies = await getPoloniexCurrencies();
  const coinMarketCap = await getCoinMarketCoins();

  const binanceCurrencies = currencies[0].currencies;

  // console.log(binanceCurrencies.length);
  console.log(Object.keys(poloniexCurrencies).filter(o => poloniexCurrencies[o].delisted === 0 && poloniexCurrencies[o].disabled === 0).length);


  let count = 0;

  Object.keys(poloniexCurrencies).filter(o => poloniexCurrencies[o].delisted === 0 && poloniexCurrencies[o].disabled === 0).forEach(o => {
    if(!binanceCurrencies.find(p => p.id === o)){
      // if(!coinMarketCap.data.find(p => p.symbol === o)){
        console.log(o);
      // }
      // else count ++;
    } 
  })

  console.log(count);
}

getInfo();