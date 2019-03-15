import React from 'react'
import Asset from './Asset'

const ExchangeBlock = ({
  exchange,
  currencies
}) => {
  return (
    //TODO : Check if Free > 0; if not => red color with infobox onHover 
    <div className='exchangeBlock'>
      <h4>{exchange}</h4>
      <div className='assetWrapper'>
        {currencies.map(o => <Asset key={o.id} {...o} />)}
      </div>
    </div>
  )
}


export default ExchangeBlock
