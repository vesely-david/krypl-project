import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { overviewActions } from '../../actions/overviewActions'
// import StrategyPage from '../../../components/StrategyPage'

class RealContainer extends React.Component {
  componentDidMount () {
    this.props.overviewActions.getPaperOverview()
    this.props.overviewActions.getPaperStrategies()
  }

  render () {
    const {
      overviewActions:{forgetAllPaperNews, forgetPaperNews, registerPaper },
      paper
    } = this.props
    return null;
    // return (
    //   <StrategyPage
    //     data={paper}
    //     actions={{
    //       fetchStrategies: fetchPaperStrategies,
    //       fetchOverview: fetchPaperOverview,
    //       forgetAllNews: forgetAllPaperNews,
    //       forgetNews: forgetPaperNews,
    //       registerStrategy: registerPaper
    //     }}
    //   />
    // )
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    overviewActions: bindActionCreators(overviewActions, dispatch),
  }
}

const mapStateToProps = (state) => ({
  paper: state.paper,
})

export default connect(mapStateToProps, mapDispatchToProps)(RealContainer)
