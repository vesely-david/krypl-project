import React from 'react'
import { connect } from 'react-redux'

const BacktestContainer = () => {
  return (
    <h1>Backtest</h1>
  )
}

const mapDispatchToProps = {
}

const mapStateToProps = (state) => ({
})

export default connect(mapStateToProps, mapDispatchToProps)(BacktestContainer)
