import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
import OverviewContainer from '../../components/Overview/OverviewContainer'
import NewStrategyModal from '../modals/NewStrategyModal';
import { getGroupedAssets } from '../../selectors/assetSelectors';
import styles from '../styles/subpages.module.scss';
import { getRealStrategies } from '../../selectors/strategySelector';
import { getRealOverview } from '../../selectors/overviewSelectors';

class RealContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getRealOverview();
    this.props.overviewActions.getRealStrategies();
  }

  render () {
    const {
      overviewActions:{ registerRealStrategy },
      registrationPending,
      strategiesFetching,
      overviewFetching,
      realOverview,
      realStrategies,
      groupedAssets:{
        groupedRealAssets
      },
      history,
      historyFetching
    } = this.props;

    return (
      <div className={styles.app}>
        <OverviewContainer
          overview={realOverview}
          registrationPending={registrationPending}
          strategiesFetching={strategiesFetching}
          overviewFetching={overviewFetching}
          strategies={realStrategies}
          forgetAllNews={() => alert('forget')}
          history={history}
          historyFetching={historyFetching}          
          addStrategyModal={(
            <NewStrategyModal
              registrationPending={registrationPending}
              registerStrategy={registerRealStrategy}
              allAssets={groupedRealAssets}
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
  registrationPending: state.real.registrationPending,
  strategiesFetching: state.real.strategiesFetching,
  overviewFetching: state.real.overviewFetching,
  historyFetching: state.real.historyFetching,
  history: state.real.history,  
  realOverview: getRealOverview(state),
  realStrategies: getRealStrategies(state),
  groupedAssets: getGroupedAssets(state),
})

export default connect(mapStateToProps, mapDispatchToProps)(RealContainer)
