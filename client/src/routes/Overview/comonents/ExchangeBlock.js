import React from 'react'
import PropTypes from 'prop-types'
import Asset from './Asset'
import '../styles/ExchangeBlock.scss'

const ExchangeBlock = ({
  exchange,
  assets
}) => {
  return (
    //TODO : Check if Free > 0; if not => red color with infobox onHover 
    <div className='exchangeBlock'>
      <h4>{exchange}</h4>
      <div className='assetWrapper'>
        {assets.map(o => <Asset key={o.value} asset={o} />)}
      </div>
    </div>
  )
}

ExchangeBlock.propTypes = {
  exchange: PropTypes.string,
  assets: PropTypes.array
}

export default ExchangeBlock
