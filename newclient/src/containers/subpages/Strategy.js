import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { strategyActions } from '../../actions/strategyActions';
// import TradeList from '../components/TradeList'
// import StrategyOverview from '../components/StrategyOverview'

class StrategyContainer extends React.Component {
  componentDidMount () {
    var id = this.props.match.params.strategyId
    this.props.strategyActions.getStrategyData(id)
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
        {/* <StrategyOverview overview={overview} isFetching={overviewFetching} /> */}
        {/* <TradeList tradeList={trades} isFetching={tradesFetching} /> */}
      </div>
    )
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    strategyActions: bindActionCreators(strategyActions, dispatch)
  }
}
const mapStateToProps = (state) => {
  // const {
  //   overview,
  //   trades,
  //   overviewFetching,
  //   tradesFetching,
  // } = state.strategy
  // return {
  //   overview,
  //   trades,
  //   overviewFetching,
  //   tradesFetching,
  // }
}
export default connect(mapStateToProps, mapDispatchToProps)(StrategyContainer)
