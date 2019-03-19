import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { strategyActions } from '../../actions/strategyActions';
import TradeList from '../../components/Strategy/TradeList'
import StrategyOverview from '../../components/Strategy/StrategyOverview'
import styles from '../styles/subpages.module.scss';

class StrategyContainer extends React.Component {
  componentDidMount () {
    this.props.strategyActions.getStrategyData(this.props.match.params.strategyId)
  }

  render () {
    const {
      overviews,
      trades,
      histories,
      strategyOverviewFetching,
      strategyTradesFetching,
      historyFetching,
    } = this.props;
    const id = this.props.match.params.strategyId
    return (
      <div className={styles.app}>
        <StrategyOverview 
          overview={overviews[id]} 
          isFetching={strategyOverviewFetching} 
          strategyId={id}
          history={histories[id]}
          isHistoryFetching={historyFetching}
        />
        <TradeList tradeList={trades[id]} isFetching={strategyTradesFetching} />
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
  return{
    ...state.strategies,
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(StrategyContainer)
