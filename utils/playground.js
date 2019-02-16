const fetch = require('node-fetch');




const getSymbolsInfo = async () => {
  const info = await fetch('https://www.binance.com/api/v1/exchangeInfo', {method: 'GET'})
  const infoJson = await info.json();

  var result = infoJson.symbols.reduce((res, val) => {
    if(res[val.baseAsset] === undefined) res[val.baseAsset] = [];
    res[val.baseAsset].push(val.quoteAsset);
    return res;
  }, {});
  for(var a in result){
    if(result[a].indexOf('BTC') === -1) console.log(a);
  }
}


getSymbolsInfo();