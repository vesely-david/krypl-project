import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/overview'
import { Segment, Button } from 'semantic-ui-react'
import RealAssetManagerModal from './RealAssetManagerModal'
import './styles/overviewContainer.scss'

class OverviewContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchExchangesOverview()
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
      isRealDataFetching
    } = this.props
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
        </Segment>
        <Segment color='teal' className='mainOverviewSegment'>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Paper Assets</h3>
            <Button color='teal'>Asset management</Button>
          </div>
        </Segment>
        <Segment color='blue' className='mainOverviewSegment'>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Backtest Assets</h3>
            <Button color='blue'>Asset management</Button>
          </div>
        </Segment>
      </div>
    )
  }
}

OverviewContainer.propTypes = {
  actions: PropTypes.object,
  isRealDataFetching: PropTypes.bool,
  exchangesOverview: PropTypes.object,
  exchangesOverviewFetching: PropTypes.bool,
  mirrorExchangeAssetsFetching: PropTypes.bool,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(actions, dispatch)
  }
}

const mapStateToProps = (state) => {
  return {
    realAssets:                   state.real.overview.assets,
    isRealDataFetching:           state.real.overviewFetching,
    exchangesOverview:             state.overview.exchangesOverview,
    exchangesOverviewFetching:     state.overview.exchangesOverviewFetching,
    mirrorExchangeAssetsFetching: state.overview.mirrorExchangeAssetsFetching
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
