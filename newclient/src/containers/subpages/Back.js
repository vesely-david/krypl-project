import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
import BacktestOverviewContainer from '../../components/Overview/BacktestOverviewContainer'
import NewBacktestStrategyModal from '../modals/NewBacktestStrategyModal';
import styles from '../styles/subpages.module.scss';
import { getRawBacktestStrategies } from '../../selectors/strategySelector';
import { getRawBackOverview } from '../../selectors/overviewSelectors';
import { getMarketData } from '../../selectors/marketDataSelectors';

class BackContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getBacktestOverview()
    this.props.overviewActions.getBacktestStrategies()
  }

  render () {
    const {
      overviewActions:{ registerBacktestStrategy },
      registrationPending,
      strategiesFetching,
      overviewFetching,
      backtestOverview,
      backtestStrategies,
      marketData,
    } = this.props;
    return (
      <div className={styles.app}>
        <BacktestOverviewContainer
          overview={backtestOverview}
          registrationPending={registrationPending}
          strategiesFetching={strategiesFetching}
          overviewFetching={overviewFetching}
          strategies={backtestStrategies}
          addStrategyModal={(
            <NewBacktestStrategyModal
              registrationPending={registrationPending}
              registerStrategy={registerBacktestStrategy}
              marketData={marketData}
            />
          )}
        />
      </div>
    )
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    overviewActions: bindActionCreators(overviewActions, dispatch),
  }
}

const mapStateToProps = (state) => ({
  registrationPending: state.back.registrationPending,
  strategiesFetching: state.back.strategiesFetching,
  overviewFetching: state.back.overviewFetching,
  backtestOverview: getRawBackOverview(state),
  backtestStrategies: getRawBacktestStrategies(state),
  marketData: getMarketData(state),
})

export default connect(mapStateToProps, mapDispatchToProps)(BackContainer)
