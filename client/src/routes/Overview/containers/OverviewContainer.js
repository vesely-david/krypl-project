import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/overview'
import { Segment } from 'semantic-ui-react'
import AssetManagerModal from './AssetManagerModal'

class OverviewContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchMainOverview()
  }

  render () {
    const {
      realOverview,
      fetchAssetOptions,
      assetOptions,
      assetOptionsFetching,
      assetUpdateFetching
    } = this.props
    return (
      <div className='mainOverview'>
        <h2>Overview</h2>
        <Segment color='green'>
          <h3>Real Assets</h3>
          <AssetManagerModal
            fakeAssets={false}
            assets={realOverview.assets}
            fetchAssetOptions={fetchAssetOptions}
            assetOptions={assetOptions}
            assetOptionsFetching={assetOptionsFetching}
            assetUpdateFetching={assetUpdateFetching}
          />
        </Segment>
        <Segment color='teal'>
          <h3>Paper Assets</h3>

        </Segment>
        <Segment color='blue'>
          <h3>Backtest Assets</h3>
        </Segment>
      </div>
    )
  }
}

OverviewContainer.propTypes = {
  actions: PropTypes.object,
  realOverview: PropTypes.object,
  assetUpdateFetching: PropTypes.bool,
  fetchAssetOptions: PropTypes.func,
  assetOptions: PropTypes.object,
  assetOptionsFetching: PropTypes.bool
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(actions, dispatch)
  }
}

const mapStateToProps = (state) => {
  return {
    realOverview: state.real.overview,
    mainOverview: state.overview,
  }
}

//({
  //realOverview: state.real.Overview,
  //paperOverview: state.paper.Overview,
  //backtestOverview: state.backtest.Overview,
//})

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
