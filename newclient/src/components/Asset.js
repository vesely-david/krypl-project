import React from 'react'

const StrategyList = ({
  currency,
  hold
}) => {
  return (
    <div className='smallPaddingLeft'>
      <div><b>{currency}</b></div>
      <div className='littlePaddingLeft'>{hold}</div>
    </div>
  )
}

export default StrategyList
