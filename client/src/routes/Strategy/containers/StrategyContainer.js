import React from 'react'
import { connect } from 'react-redux'
import TradeList from '../components/TradeList'

class StrategyContainer extends React.Component {
  render () {
    var x = this.props.params.strategyId
    return (
      <div>
        <h1>Strategy:</h1>
        <TradeList tradeList={[]} isFetching />
      </div>
    )
  }
}

const mapDispatchToProps = {
}

const mapStateToProps = (state) => ({
})

export default connect(mapStateToProps, mapDispatchToProps)(StrategyContainer)
