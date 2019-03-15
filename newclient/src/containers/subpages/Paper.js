import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
import StrategyPage from '../../components/StrategyPage/StrategyPage'
import NewStrategyModal from '../modals/NewStrategyModal';
import { getAssets, getGroupedAssets } from '../../selectors/assetSelectors';

class PaperContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getPaperOverview()
    this.props.overviewActions.getPaperStrategies()
  }

  render () {
    const {
      overviewActions:{ registerPaperStrategy },
      paper,
      assets: {
        paperAssets
      },
      groupedAssets: {
        groupedPaperAssets
      }
    } = this.props
    return (
      <StrategyPage
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
  assets: getAssets(state),
  groupedAssets: getGroupedAssets(state),
})

export default connect(mapStateToProps, mapDispatchToProps)(PaperContainer)
