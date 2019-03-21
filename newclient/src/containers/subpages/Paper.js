import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
import OverviewContainer from '../../components/Overview/OverviewContainer'
import NewStrategyModal from '../modals/NewStrategyModal';
import { getGroupedAssets } from '../../selectors/assetSelectors';
import styles from '../styles/subpages.module.scss';

class PaperContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getPaperOverview()
    this.props.overviewActions.getPaperStrategies()
  }

  render () {
    const {
      overviewActions:{ registerPaperStrategy },
      paper,
      groupedAssets: {
        groupedPaperAssets
      }
    } = this.props;

    return (
      <div className={styles.app}>
        <OverviewContainer
          data={paper}
          actions={{
            forgetAllNews: () => alert('forget'),
            registerStrategy: () => alert('register'),
          }}
          addStrategyModal={(
            <NewStrategyModal
              registrationPending={paper.registrationPending}
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
  paper: state.paper,
  groupedAssets: getGroupedAssets(state),
})

export default connect(mapStateToProps, mapDispatchToProps)(PaperContainer)
