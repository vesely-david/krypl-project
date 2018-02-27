import React from 'react'
import PropTypes from 'prop-types'
import './styles/CurrencyList.scss'
const StrategyList = ({
  currencyList,
}) => {
  return (
    <div className='currencyList'>
      {currencyList.map(o =>
        <div key={o.currency} className='currencyItem'>
          <p>{o.currency}</p>
          {o.currency !== 'Others' && <p>{`Value: ${o.value}`}</p>}
          {o.currency !== 'Btc' && <p>{`Btc value: ${o.btcValue}`}</p>}
        </div>)}
    </div>
  )
}

StrategyList.propTypes = {
  currencyList: PropTypes.array,
}

export default StrategyList
