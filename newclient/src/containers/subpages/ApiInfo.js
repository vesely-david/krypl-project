import React from 'react';
import { Segment } from 'semantic-ui-react';
import styles from '../styles/subpages.module.scss';


export default () => (
  <div className={styles.app}>
    <h2>API info</h2>        
    <Segment>
      <h3>Market data endpoints</h3>
      <h4>Supported exchanges</h4>
    </Segment>
    <Segment>
      <h3>Trading endpoints</h3>
    </Segment>    
  </div>
)