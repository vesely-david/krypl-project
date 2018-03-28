import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'
import { paperActions } from '../modules/paper'
import StrategyPage from '../../../components/StrategyPage'

class PaperContainer extends React.Component {
  componentDidMount () {
    this.props.actions.fetchPaperStrategies()
    this.props.actions.fetchPaperOverview()
  }

  render () {
    const {
      actions:{ fetchPaperStrategies, fetchPaperOverview, forgetAllPaperNews, forgetPaperNews, registerPaper },
      paper
    } = this.props
    return (
      <StrategyPage
        data={paper}
        actions={{
          fetchStrategies: fetchPaperStrategies,
          fetchOverview: fetchPaperOverview,
          forgetAllNews: forgetAllPaperNews,
          forgetNews: forgetPaperNews,
          registerStrategy: registerPaper
        }}
      />
    )
  }
}

PaperContainer.propTypes = {
  actions: PropTypes.object,
  paper: PropTypes.object,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(paperActions, dispatch)
  }
}

const mapStateToProps = (state) => ({
  paper: state.paper,
})

export default connect(mapStateToProps, mapDispatchToProps)(PaperContainer)
