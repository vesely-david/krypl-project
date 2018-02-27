import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { actions } from '../modules/overview'

class OverviewContainer extends React.Component {
  componentDidMount () {
    //this.props.actions.fetchMainOverview()
  }

  render () {
    return (
      <div className='mainOverview'>
        <h1>Overview</h1>
      </div>
    )
  }
}

OverviewContainer.propTypes = {
  actions: PropTypes.object
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(actions, dispatch)
  }
}

const mapStateToProps = (state) => ({
  realOverview: state.real.Overview,
  paperOverview: state.paper.Overview,
  backtestOverview: state.backtest.Overview,
})

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
