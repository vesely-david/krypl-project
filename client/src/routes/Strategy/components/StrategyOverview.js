import React from 'react'
import PropTypes from 'prop-types'
import DimmableSegment from '../../../components/DimmableSegment'
import EvaluationGraph from './EvaluationGraph'

const StrategyOverview = ({ overview, isFetching }) => {
  return (
    <DimmableSegment active={isFetching}>
      <h2>Test</h2>
      {overview.evaluations && overview.evaluations.length > 0 && (
        <EvaluationGraph data={overview.evaluations} />
      )}
    </DimmableSegment>
  )
}

StrategyOverview.propTypes = {
  overview: PropTypes.object,
  isFetching: PropTypes.bool,
}

export default StrategyOverview
