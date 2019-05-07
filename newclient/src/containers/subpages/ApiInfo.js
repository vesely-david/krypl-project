import React from 'react';
import { Segment } from 'semantic-ui-react';
import styles from '../styles/subpages.module.scss';


export default () => (
  <div className={styles.app}>
    <h2>API info</h2>        
    <Segment>
      <h3>Market data endpoints</h3>
      <Segment>
        <h4>Actual info</h4>
      </Segment>
      <Segment>
        <h4>History</h4>
      </Segment>
      <h4>Supported exchanges</h4>
    </Segment>
    <Segment>
      <h3>Strategy info endpoints</h3>
      <div>
        <div>
          <h4>Strategy Assets</h4>
        </div>
        <div>
          <h4>Strategy Trades</h4>
        </div>
      </div>
    </Segment>        
    <Segment>
      <h3>Trading endpoints</h3>
    </Segment>    
  </div>
)