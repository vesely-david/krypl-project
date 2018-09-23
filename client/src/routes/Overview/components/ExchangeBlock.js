import React from 'react'
import PropTypes from 'prop-types'
import Asset from './Asset'
import '../styles/ExchangeBlock.scss'

const ExchangeBlock = ({
  exchange,
  currencies
}) => {
  return (
    //TODO : Check if Free > 0; if not => red color with infobox onHover 
    <div className='exchangeBlock'>
      <h4>{exchange}</h4>
      <div className='assetWrapper'>
        {currencies.map(o => <Asset key={o.id} asset={o} />)}
      </div>
    </div>
  )
}

ExchangeBlock.propTypes = {
  exchange: PropTypes.string,
  currencies: PropTypes.array
}

export default ExchangeBlock
