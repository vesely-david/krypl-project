import React from 'react'
import PropTypes from 'prop-types'

const StrategyList = ({
  asset: {
    text,
    sum
  }
}) => {
  return (
    <div className='smallPaddingLeft'>
      <div><b>{text}</b></div>
      <div className='littlePaddingLeft'>{sum}</div>
    </div>
  )
}

StrategyList.propTypes = {
  asset: PropTypes.object
}

export default StrategyList
