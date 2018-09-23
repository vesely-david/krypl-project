import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/strategy'
import TradeList from '../components/TradeList'
import StrategyOverview from '../components/StrategyOverview'

class StrategyContainer extends React.Component {
  componentDidMount () {
    var id = this.props.params.strategyId
    this.props.actions.fetchStrategyOverview(id)
    this.props.actions.fetchStrategyTrades(id)
  }

  render () {
    const {
      overview,
      trades,
      overviewFetching,
      tradesFetching,
    } = this.props
    
    return (
      <div>
        <StrategyOverview overview={overview} isFetching={overviewFetching} />
        <TradeList tradeList={trades} isFetching={tradesFetching} />
      </div>
    )
  }
}

StrategyContainer.propTypes = {
  actions: PropTypes.object,
  params: PropTypes.object,
  overview: PropTypes.object,
  trades: PropTypes.array,
  overviewFetching: PropTypes.bool,
  tradesFetching: PropTypes.bool,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(actions, dispatch)
  }
}
const mapStateToProps = (state) => {
  const {
    overview,
    trades,
    overviewFetching,
    tradesFetching,
  } = state.strategy
  return {
    overview,
    trades,
    overviewFetching,
    tradesFetching,
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(StrategyContainer)
