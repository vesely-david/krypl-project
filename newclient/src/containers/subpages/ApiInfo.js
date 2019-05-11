import React from 'react';
import { Segment } from 'semantic-ui-react';
import styles from '../styles/subpages.module.scss';


export default () => (
  <div className={styles.app}>
    <h2>API info</h2>        
    <Segment>
      <h3>Market data endpoints</h3>
      <h5>Base URL: https://marketdata.jankirchner.cz/</h5>
      <Segment>
        <h4>Actual info</h4>
        <Segment>
          <h4>Supported Exchanges & trading Pairs</h4>
          <h5>{'Request: GET /exchanges'}</h5>
          <h5>Response:</h5>
          <div>{'[{'}</div>
          <div className={styles.paddingLeft}>{'"id": "binance",'}</div>
          <div className={styles.paddingLeft}>{'"name": "Binance",'}</div>
          <div className={styles.paddingLeft}>{'"history": true,'}</div>
          <div className={styles.paddingLeft}>{'"currencies": ['}</div>
          <div className={styles.paddingLeft}>
            <div className={styles.paddingLeft}>{'{'}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'"id": "ADA",'}</div>
                <div className={styles.paddingLeft}>{'"name": "Cardano",'}</div>
                <div className={styles.paddingLeft}>{'"currencyExchangeId": "ADA",'}</div>
              </div>
            <div className={styles.paddingLeft}>{'},'}</div>
            <div className={styles.paddingLeft}>{'{'}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'"id": "ADX",'}</div>
                <div className={styles.paddingLeft}>{'"name": "AdEx",'}</div>
                <div className={styles.paddingLeft}>{'"currencyExchangeId": "ADX",'}</div>
              </div>
            <div className={styles.paddingLeft}>{'},'}</div>  
            <div className={styles.paddingLeft}>{'{...}'}</div>  
          </div>
          <div className={styles.paddingLeft}>{']'}</div>
          <div>{'}, {...}]'}</div>          
        </Segment>
        <Segment>
          <h4>Actual rates</h4>
          <h5>{'Request: GET business/rate/{exchangeId}'}</h5>
          <h5>Response:</h5>
          <div>{'['}</div>
          <div className={styles.paddingLeft}>
            <div className={styles.paddingLeft}>{'{'}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'"symbol": "BTC_LTC",'}</div>
                <div className={styles.paddingLeft}>{'"rate": 0.01264100'}</div>
              </div>
            <div className={styles.paddingLeft}>{'},'}</div>
            <div className={styles.paddingLeft}>{'{'}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'"symbol": "BTC_BNB",'}</div>
                <div className={styles.paddingLeft}>{'"rate": 0.00367700'}</div>
              </div>
            <div className={styles.paddingLeft}>{'},'}</div>
            <div className={styles.paddingLeft}>{'{...}'}</div>
          </div>
          <div>{']'}</div>
        </Segment>
        <Segment>
          <h4>Orderbook</h4>
          <h5>{'Request: GET business/orderbook/{exchangeId}/{marketId}'}</h5>
          <h5>Response:</h5>
          <div>{'{'}</div>
            <div className={styles.paddingLeft}>{'asks: ['}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'{'}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"price": 0.01131788,'}</div>
                    <div className={styles.paddingLeft}>{'"orders": 0.02302116'}</div>
                  </div>
                <div className={styles.paddingLeft}>{'},'}</div>
                <div className={styles.paddingLeft}>{'{'}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"price": 0.01131789,'}</div>
                    <div className={styles.paddingLeft}>{'"orders": 120.44625391'}</div>
                  </div>
                <div className={styles.paddingLeft}>{'},'}</div>
                <div className={styles.paddingLeft}>{'{...}'}</div>                  
              </div>
            <div className={styles.paddingLeft}>{'],'}</div>
            <div className={styles.paddingLeft}>{'bids: ['}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'{'}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"price": 0.01131158,'}</div>
                    <div className={styles.paddingLeft}>{'"orders": 1.51717649'}</div>
                  </div>
                <div className={styles.paddingLeft}>{'},'}</div>
                <div className={styles.paddingLeft}>{'{'}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"price": 0.01130302,'}</div>
                    <div className={styles.paddingLeft}>{'"orders": 19.05144395'}</div>
                  </div>
                <div className={styles.paddingLeft}>{'},'}</div>
                <div className={styles.paddingLeft}>{'{...}'}</div>                  
              </div>
            <div className={styles.paddingLeft}>{'],'}</div>
          <div>{'}'}</div>
        </Segment>
        <Segment>
          <h4>Coin Market Cal</h4>
          <h5>{'Request: GET marketdata/marketcal'}</h5>
          <h5>Response: </h5>
          <div>{'['}</div>
          <div className={styles.paddingLeft}>
            <div className={styles.paddingLeft}>{'{'}</div>
              <div className={styles.paddingLeft}>
                <div className={styles.paddingLeft}>{'"title": "AXPR Monthly Burn",'}</div>
                <div className={styles.paddingLeft}>{'"date": "2019-05-10T00:00:00Z",'}</div>
                <div className={styles.paddingLeft}>{'"coins": ['}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"AXPR"'}</div>
                  </div>
                <div className={styles.paddingLeft}>{'],'}</div>
                <div className={styles.paddingLeft}>{'"categories": ['}</div>
                  <div className={styles.paddingLeft}>
                    <div className={styles.paddingLeft}>{'"burn"'}</div>
                  </div>
                <div className={styles.paddingLeft}>{']'}</div>                
              </div>
            <div className={styles.paddingLeft}>{'},'}</div>
            <div className={styles.paddingLeft}>{'{...}'}</div>
          </div>
          <div>{']'}</div>          
        </Segment>              
      </Segment>
      <Segment>
        <h4>History</h4>
        <Segment>
          <h4>Historical Rate</h4>
          <h5>{'Request: GET business/rate/{exchangeId}/{marketId}/{unixTimestamp}'}</h5>
          <h5>Response: Rate</h5>
        </Segment>
      </Segment>
    </Segment>
    <Segment>
      <h3>Strategy info endpoints</h3>
      <h5>Base URL: https://api.jankirchner.cz/</h5>
      <div>
        <Segment>
          <h4>Get Strategy Assets</h4>
          <h5>{'Request: GET /assets/{strategyId}'}</h5>
          <h5>Response:</h5>
          <div>{'[{'}</div>
          <div className={styles.paddingLeft}>{'"amount": 1.0,'}</div>
          <div className={styles.paddingLeft}>{'"tradingMode": 1,'}</div>
          <div className={styles.paddingLeft}>{'"currency": "BTC",'}</div>
          <div className={styles.paddingLeft}>{'"exchange": "binance",'}</div>
          <div className={styles.paddingLeft}>{'"isActive": true,'}</div>
          <div className={styles.paddingLeft}>{'"isReserved": false,'}</div>
          <div className={styles.paddingLeft}>{'"id": "2959d9f9-68d0-46b8-93f4-8a1e2efa7507"'}</div>
          <div>{'}]'}</div>
        </Segment>
        <Segment>
          <h4>Get Strategy Trades</h4>
          <h5>{'Request: GET trade/{strategyId}/trades'}</h5>
          <h5>Response:</h5>
          <div>{'[{'}</div>
          <div className={styles.paddingLeft}>{'"marketId": "BTC_BAT",'}</div>
          <div className={styles.paddingLeft}>{'"quantity": 12704.86596,'}</div>
          <div className={styles.paddingLeft}>{'"quantityRemaining": 0'}</div>
          <div className={styles.paddingLeft}>{'"opened": "2019-04-24T17:42:55.506478",'}</div>
          <div className={styles.paddingLeft}>{'"closed": "2019-04-24T17:43:29.465988",'}</div>
          <div className={styles.paddingLeft}>{'"tradeState": 4,'}</div>
          <div className={styles.paddingLeft}>{'"total": 1.0,'}</div>
          <div className={styles.paddingLeft}>{'"orderType": 1,'}</div>
          <div className={styles.paddingLeft}>{'"reservedAssetId": null,'}</div>
          <div className={styles.paddingLeft}>{'"id": "db3fb8cf-4388-4e1c-a11a-988ff0dbf033"'}</div>
          <div>{'}]'}</div>
        </Segment>                   
        <Segment>
          <h4>Get Strategy Trade</h4>
          <h5>{'Request: GET trade/{tradeId}'}</h5>
          <h5>Response:</h5>
          <div>{'{'}</div>
          <div className={styles.paddingLeft}>{'"marketId": "BTC_BAT",'}</div>
          <div className={styles.paddingLeft}>{'"quantity": 12704.86596,'}</div>
          <div className={styles.paddingLeft}>{'"quantityRemaining": 0'}</div>
          <div className={styles.paddingLeft}>{'"opened": "2019-04-24T17:42:55.506478",'}</div>
          <div className={styles.paddingLeft}>{'"closed": "2019-04-24T17:43:29.465988",'}</div>
          <div className={styles.paddingLeft}>{'"tradeState": 4,'}</div>
          <div className={styles.paddingLeft}>{'"total": 1.0,'}</div>
          <div className={styles.paddingLeft}>{'"orderType": 1,'}</div>
          <div className={styles.paddingLeft}>{'"reservedAssetId": null,'}</div>
          <div className={styles.paddingLeft}>{'"id": "db3fb8cf-4388-4e1c-a11a-988ff0dbf033"'}</div>
          <div>{'}'}</div>             
        </Segment>            
      </div>
    </Segment>        
    <Segment>
      <h3>Trading endpoints</h3>
      <h5>Base URL: https://api.jankirchner.cz</h5>
      <div>
        <Segment>
          <h4>Post Buy Order</h4>
          <h5></h5>
          <h5>{'Request: POST /trade/{strategyId}/buy'}</h5>
          <h5>Body:</h5>
          <div>{'{'}</div>
          <div className={styles.paddingLeft}>{'"exchange": "poloniex",'}</div>
          <div className={styles.paddingLeft}>{'"symbol": "BTC_LTC",'}</div>
          <div className={styles.paddingLeft}>{'"amount": 1,'}</div>
          <div className={styles.paddingLeft}>{'"rate": 0.01'}<span className={styles.notRequired}>Current price if not present</span></div>
          <div>{'}'}</div>
          <h5>Response: TradeId</h5>
        </Segment>
        <Segment>
          <h4>Post Sell Order</h4>
          <h5>{'Request: POST /trade/{strategyId}/sell'}</h5>
          <h5>Body:</h5>
          <div>{'{'}</div>
          <div className={styles.paddingLeft}>{'"exchange": "poloniex",'}</div>
          <div className={styles.paddingLeft}>{'"symbol": "BTC_LTC",'}</div>
          <div className={styles.paddingLeft}>{'"amount": 1,'}</div>
          <div className={styles.paddingLeft}>{'"rate": 0.01'}<span className={styles.notRequired}>Current price if not present</span></div>
          <div>{'}'}</div>      
          <h5>Response: TradeId</h5>
        </Segment>  
        <Segment>
          <h4>Cancel Order</h4>
          <h5>{'Request: DELETE /trade/{tradeId}'}</h5>
          <h5>Response: TradeId</h5>
        </Segment>
        <Segment>
          <h4>Backtesting</h4>
          <Segment>
            <h4>Post Historical Buy Order </h4>
            <h5></h5>
            <h5>{'Request: POST /trade/{strategyId}/buy/{timestamp}'}</h5>
            <h5>Body:</h5>
            <div>{'{'}</div>
            <div className={styles.paddingLeft}>{'"exchange": "poloniex",'}</div>
            <div className={styles.paddingLeft}>{'"symbol": "BTC_LTC",'}</div>
            <div className={styles.paddingLeft}>{'"amount": 1,'}</div>
            <div className={styles.paddingLeft}>{'"rate": 0.01'}<span className={styles.notRequired}>Historical price if not present</span></div>
            <div>{'}'}</div>
            <h5>Response: TradeId</h5>
          </Segment>
          <Segment>
            <h4>Post Historical Sell Order</h4>
            <h5>{'Request: POST /trade/{strategyId}/sell/{timestamp}'}</h5>
            <h5>Body:</h5>
            <div>{'{'}</div>
            <div className={styles.paddingLeft}>{'"exchange": "poloniex",'}</div>
            <div className={styles.paddingLeft}>{'"symbol": "BTC_LTC",'}</div>
            <div className={styles.paddingLeft}>{'"amount": 1,'}</div>
            <div className={styles.paddingLeft}>{'"rate": 0.01'}<span className={styles.notRequired}>Historical price if not present</span></div>
            <div>{'}'}</div>      
            <h5>Response: TradeId</h5>
          </Segment>      
          <Segment>
            <h4>Cancel Historical Order</h4>
            <h5>{'Request: DELETE /trade/{tradeId}/{timestamp}'}</h5>
            <h5>Response: TradeId</h5>
          </Segment>          
        </Segment>          
      </div>      
    </Segment>    
  </div>
)