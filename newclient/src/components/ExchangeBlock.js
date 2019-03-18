import React from 'react'
import Asset from './Asset'

import styles from './styles/home.module.scss';

const ExchangeBlock = ({
  exchange,
  currencies
}) => {
  return (
    //TODO : Check if Free > 0; if not => red color with infobox onHover 
    <div className={styles.exchange_block}>
      <h4>{exchange}</h4>
      <div className={styles.asset_wrapper}>
        {currencies.map(o => <Asset key={o.id} {...o} />)}
      </div>
    </div>
  )
}


export default ExchangeBlock
