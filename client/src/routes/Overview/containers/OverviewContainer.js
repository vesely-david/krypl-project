import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/overview'
import { Segment, Button } from 'semantic-ui-react'
import RealAssetManagerModal from './RealAssetManagerModal'
import FakeAssetManagerModal from './FakeAssetManagerModal'
import ExchangeBlock from '../comonents/ExchangeBlock'
import '../styles/OverviewContainer.scss'

class OverviewContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchExchangesOverview()
    this.props.actions.fetchAllOverviews()
  }

  render () {
    const {
      actions,
      exchangesOverview: {
        real,
        paper,
        backtest
      },
      exchangesOverviewFetching,
      mirrorExchangeAssetsFetching,
      submitPaperAssetsFetching,
      realAssets,
      paperAssets,
      isRealDataFetching,
      isPaperDataFetching,
    } = this.props
    // debugger;
    return (
      <div className='mainOverview'>
        <h2>Overview</h2>
        <Segment color='green' className='mainOverviewSegment' loading={isRealDataFetching}>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Real Assets</h3>
            <RealAssetManagerModal
              assetOptions={real}
              mirrorExchangeAssets={actions.mirrorExchangeAssets}
              mirrorExchangeAssetsFetching={mirrorExchangeAssetsFetching}
              exchangesOverviewFetching={exchangesOverviewFetching}
            />
          </div>
          {realAssets.length > 0 && <div className='divider' />}
          {realAssets.map(o =>
            <ExchangeBlock exchange={o.text} assets={o.assets} key={o.value} />
          )}
        </Segment>
        <Segment color='teal' className='mainOverviewSegment' loading={isPaperDataFetching}>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Paper Assets</h3>
            <FakeAssetManagerModal
              color='teal'
              assets={paperAssets}
              assetOptions={paper}
              submitAssets={actions.submitPaperAssets}
              submitAssetsFetching={submitPaperAssetsFetching}
              exchangesOverviewFetching={exchangesOverviewFetching}
            />
          </div>
          {paperAssets.length > 0 && <div className='divider' />}
          {paperAssets.map(o =>
            <ExchangeBlock exchange={o.text} assets={o.assets} key={o.value} />
          )}
        </Segment>
        <Segment color='blue' className='mainOverviewSegment'>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Backtest Assets</h3>
            <Button color='blue'>Manage assets</Button>
          </div>
        </Segment>
      </div>
    )
  }
}

OverviewContainer.propTypes = {
  actions: PropTypes.object,
  realAssets: PropTypes.array,
  paperAssets: PropTypes.array,
  isRealDataFetching: PropTypes.bool,
  isPaperDataFetching: PropTypes.bool,
  exchangesOverview: PropTypes.object,
  exchangesOverviewFetching: PropTypes.bool,
  mirrorExchangeAssetsFetching: PropTypes.bool,
  submitPaperAssetsFetching: PropTypes.bool,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(actions, dispatch)
  }
}

const mapStateToProps = (state) => {
  return {
    realAssets:                    state.real.overview.assets,
    paperAssets:                   state.paper.overview.assets,
    isRealDataFetching:            state.real.overviewFetching,
    isPaperDataFetching:           state.paper.overviewFetching,
    exchangesOverview:             state.overview.exchangesOverview,
    exchangesOverviewFetching:     state.overview.exchangesOverviewFetching,
    mirrorExchangeAssetsFetching:  state.overview.mirrorExchangeAssetsFetching,
    submitPaperAssetsFetching:     state.overview.submitPaperAssetsFetching
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
