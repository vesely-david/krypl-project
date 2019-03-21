import React from 'react'

const StrategyList = ({
  currency,
  free,
  taken,
}) => {
  return (
    <div className='smallPaddingLeft'>
      <div><b>{currency}</b><span style={{marginLeft: '5px'}}>{free + taken}</span></div>
      <div className='littlePaddingLeft'>{`free: ${free}`}</div>
    </div>
  )
}

export default StrategyList
