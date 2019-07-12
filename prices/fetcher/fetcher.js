const WebSocket = require('ws');
const axios = require('axios');
const moment = require('moment');
var redisClient = require("redis").createClient({
  host: 'redis',
  port: 6379
});
var CronJob = require('cron').CronJob;
var currencyPairIds = require('./currencyPairIds.js')
const channel = 1002;

let oneMinuteObj = {};
let thirtyMinuteObj = {};

let translationObj = {};
let currencyList = [];

const minuteJob = new CronJob('0 */1 * * * *', () => sendCandles(oneMinuteObj, '1min'));
const thirtyMinuteJob = new CronJob('0 */30 * * * *', () => sendCandles(thirtyMinuteObj, '30min'));

const sendCandles = (candleObj, channelId) => {
  console.log(`${ channelId} event fired.`)
  const objToSend = currencyList.reduce((res, val) => {
    res[translationObj[val]] = candleObj[val];
    return res;
  }, {})

  redisClient.publish(channelId, JSON.stringify(objToSend))

  const timestamp = moment.utc().valueOf();
  currencyList.forEach(o => {
    if(candleObj[o]){
      candleObj[o].timestamp = timestamp;
      candleObj[o].open = candleObj[o].close;
      candleObj[o].min = candleObj[o].close;
      candleObj[o].max = candleObj[o].close;
    }
  })
}

const createTranslationObj = async () => {
  const poloniexData = (await axios.get('https://marketdataprovider/exchanges/poloniex')).data;
  poloniexData.markets.forEach(o => {
    if(currencyPairIds[o.marketExchangeId]){
      translationObj[currencyPairIds[o.marketExchangeId]] = o.id;
      currencyList.push(currencyPairIds[o.marketExchangeId]);
    }
  });
}

const writePrice = (candleObj, id, price) => {
  if(!candleObj[id]){
    candleObj[id] = {
      min: price, 
      max: price,
      open: price,
      close: price,
      timestamp: moment.utc().valueOf()
    }
  } else{
    if(price < candleObj[id].min) candleObj[id].min = price;
    if(price > candleObj[id].max) candleObj[id].max = price;
    candleObj[id].close = price;
  }
}

function init(){
  createTranslationObj();
  const ws = new WebSocket('wss://api2.poloniex.com');
 
  ws.on('open', function open() {
    ws.send(JSON.stringify({
      command: 'subscribe',
      channel: channel
    }))
  });
  
  ws.on('message', data => {
    var json = JSON.parse(data);

    if(json[0] != channel || json[1] !== null) return;
    const id = json[2][0];
    const price = json[2][1];

    writePrice(oneMinuteObj, id, price);
    writePrice(thirtyMinuteObj, id, price);
  });

  minuteJob.start();
  thirtyMinuteJob.start();
}

init();
console.log('Fetcher started');
