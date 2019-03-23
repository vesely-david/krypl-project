import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { strategyActions } from '../../actions/strategyActions';
import TradeList from '../../components/Strategy/TradeList'
import StrategyOverview from '../../components/Strategy/StrategyOverview'
import styles from '../styles/subpages.module.scss';
import { getAllAssets } from '../../selectors/assetSelectors';
import { getStrategyCurrentValues, getAllStrategies } from '../../selectors/strategySelector';

class StrategyContainer extends React.Component {

  constructor(props){
    super(props);
    this.state = {
      hovered: null,
    }
  }
  componentDidMount () {
    const id = this.props.match.params.strategyId;
    if(!this.props.strategies[id]) this.props.strategyActions.getStrategy(id, this.props.match.params.tradingMode)
    this.props.strategyActions.getStrategyData(id)
  }

  render () {
    const {
      strategies,
      trades,
      histories,
      strategyOverviewFetching,
      strategyTradesFetching,
      historyFetching,
      assets,
      strategyActions,
    } = this.props;

    const id = this.props.match.params.strategyId;
    const strategyAssets = assets.filter(o => o.strategyId === id);
    return (
      <div className={styles.app}>
        <StrategyOverview 
          overview={strategies[id]} 
          isFetching={false} //TODO 
          history={histories[id]}
          isHistoryFetching={historyFetching}
          strategyAssets={strategyAssets}
          hovered={this.state.hovered}
          stopStrategy={strategyActions.stopStrategy}
        />
        <TradeList tradeList={trades[id]} isFetching={strategyTradesFetching} onTradeHover={(date) => this.setState({hovered: date})}/>
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
    assets: getAllAssets(state),
    currentStrategyValues: getStrategyCurrentValues(state),
    strategies: getAllStrategies(state),
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(StrategyContainer)
