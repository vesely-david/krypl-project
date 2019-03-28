import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
import OverviewContainer from '../../components/Overview/OverviewContainer'
import NewStrategyModal from '../modals/NewStrategyModal';
import { getGroupedAssets } from '../../selectors/assetSelectors';
import styles from '../styles/subpages.module.scss';
import { getPaperStrategies } from '../../selectors/strategySelector';
import { getPaperOverview } from '../../selectors/overviewSelectors';

class PaperContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getPaperOverview()
    this.props.overviewActions.getPaperStrategies()
  }

  render () {
    const {
      overviewActions:{ registerPaperStrategy },
      registrationPending,
      strategiesFetching,
      overviewFetching,
      paperOverview,
      paperStrategies,
      groupedAssets:{
        groupedPaperAssets
      },
      historyFetching,
      history,
    } = this.props;
    console.log(historyFetching);
    console.log(history);
    return (
      <div className={styles.app}>
        <OverviewContainer
          overview={paperOverview}
          registrationPending={registrationPending}
          strategiesFetching={strategiesFetching}
          overviewFetching={overviewFetching}
          strategies={paperStrategies}
          history={history}
          historyFetching={historyFetching}
          forgetAllNews={() => alert('forget')}
          addStrategyModal={(
            <NewStrategyModal
              registrationPending={registrationPending}
              registerStrategy={registerPaperStrategy}
              allAssets={groupedPaperAssets}
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
  registrationPending: state.paper.registrationPending,
  strategiesFetching: state.paper.strategiesFetching,
  overviewFetching: state.paper.overviewFetching,
  historyFetching: state.paper.historyFetching,
  history: state.paper.history,
  paperOverview: getPaperOverview(state),
  paperStrategies: getPaperStrategies(state),
  groupedAssets: getGroupedAssets(state),
})

export default connect(mapStateToProps, mapDispatchToProps)(PaperContainer)
