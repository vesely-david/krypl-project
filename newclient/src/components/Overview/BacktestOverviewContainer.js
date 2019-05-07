import React from 'react'
import BacktestSmallOverview from './BacktestSmallOverview'
import StrategyList from './StrategyList'

const BacktestOverviewContainer = ({
  overview,
  registrationPending,
  strategiesFetching,
  overviewFetching,
  strategies,
  addStrategyModal,
}) => {
  return (
    <div>
      <BacktestSmallOverview
        isFetching={overviewFetching}
        registrationPending={registrationPending}
        overview={overview}
        addStrategyModal={addStrategyModal}
      />
      <StrategyList
        strategies={strategies}
        isFetching={strategiesFetching}
        isBacktesting={true}
      />
    </div>)
  }

export default BacktestOverviewContainer
