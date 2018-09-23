import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/overview'
import { Segment, Button } from 'semantic-ui-react'
import RealAssetManagerModal from './RealAssetManagerModal'
import FakeAssetManagerModal from './FakeAssetManagerModal'
import ExchangeBlock from '../components/ExchangeBlock'
import { commonMarketDataActions } from '../../../commonModules/commonMarketData'
import '../styles/OverviewContainer.scss'

class OverviewContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchCommon()
    this.props.actions.fetchAllOverviews()
  }

  render () {
    const {
      actions,
      commonMarketDataFetching,
      commonMarketData,
      mirrorExchangeAssetsFetching,
      submitPaperAssetsFetching,
      realAssets,
      paperAssets,
      isRealDataFetching,
      isPaperDataFetching,
    } = this.props
    return (
      <div className='mainOverview'>
        <h2>Overview</h2>
        <Segment color='green' className='mainOverviewSegment' loading={isRealDataFetching}>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Real Assets</h3>
            <RealAssetManagerModal
              marketData={commonMarketData}
              marketDataFetching={commonMarketDataFetching}
              mirrorExchangeAssets={actions.mirrorExchangeAssets}
              mirrorExchangeAssetsFetching={mirrorExchangeAssetsFetching}
            />
          </div>
          {realAssets && realAssets.length > 0 && <div className='divider' />}
          {realAssets.map(o =>
            <ExchangeBlock exchange={o.name} currencies={o.currencies} key={o.id} />
          )}
        </Segment>
        <Segment color='teal' className='mainOverviewSegment' loading={isPaperDataFetching}>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Paper Assets</h3>
            <FakeAssetManagerModal
              color='teal'
              assets={paperAssets}
              marketData={commonMarketData}
              marketDataFetching={commonMarketDataFetching}
              submitAssets={actions.submitPaperAssets}
              submitAssetsFetching={submitPaperAssetsFetching}
            />
          </div>
          {paperAssets && paperAssets.length > 0 && <div className='divider' />}
          {paperAssets.map(o =>
            <ExchangeBlock exchange={o.name} currencies={o.currencies} key={o.id} />
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
  commonMarketData: PropTypes.array,
  commonMarketDataFetching: PropTypes.bool,
  mirrorExchangeAssetsFetching: PropTypes.bool,
  submitPaperAssetsFetching: PropTypes.bool,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators({ ...actions, fetchCommon: commonMarketDataActions.fetchCommonMarketData }, dispatch)
  }
}

const mapStateToProps = (state) => {
  return {
    realAssets:                    state.real.overview.assets,
    paperAssets:                   state.paper.overview.assets,
    isRealDataFetching:            state.real.overviewFetching,
    isPaperDataFetching:           state.paper.overviewFetching,
    mirrorExchangeAssetsFetching:  state.overview.mirrorExchangeAssetsFetching,
    submitPaperAssetsFetching:     state.overview.submitPaperAssetsFetching,
    commonMarketData:              state.marketData.marketData,
    commonMarketDataFetching:      state.marketData.marketDataFetching

  }
}

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
