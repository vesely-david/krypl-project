import React from 'react'
import SmallOverview from './SmallOverview'
import StrategyList from './StrategyList'

const OverviewContainer = ({
  data: {
    overview,
    registrationPending,
    strategiesFetching,
    overviewFetching,
  },
  strategies,
  actions: {
    registerStrategy,
    forgetAllNews,
  },
  addStrategyModal,
}) => {
  return (
    <div className='strategyPage'>
      <SmallOverview
        isFetching={overviewFetching}
        registrationPending={registrationPending}
        registerStrategy={registerStrategy}
        overview={overview}
        forgetAllNews={forgetAllNews}
        addStrategyModal={addStrategyModal}
      />
      <StrategyList
        strategies={strategies}
        isFetching={strategiesFetching}
      />
    </div>)
  }

export default OverviewContainer
