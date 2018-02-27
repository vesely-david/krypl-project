import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { realActions } from '../modules/real'
import StrategyPage from '../../../components/StrategyPage'

class RealContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchRealStrategies()
    this.props.actions.fetchRealOverview()
  }

  render () {
    const {
      actions:{ fetchRealStrategies, fetchRealOverview, forgetAllRealNews, forgetRealNews, registerReal },
      real
    } = this.props
    return (
      <StrategyPage
        data={real}
        actions={{
          fetchStrategies: fetchRealStrategies,
          fetchOverview: fetchRealOverview,
          forgetAllNews: forgetAllRealNews,
          forgetNews: forgetRealNews,
          registerStrategy: registerReal
        }}
      />
    )
  }
}

RealContainer.propTypes = {
  actions: PropTypes.object,
  real: PropTypes.object,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(realActions, dispatch)
  }
}

const mapStateToProps = (state) => ({
  real: state.real,
})

export default connect(mapStateToProps, mapDispatchToProps)(RealContainer)
