import React from 'react'
import SmallOverview from './SmallOverview'
import StrategyList from './StrategyList'

const OverviewContainer = ({
  data: {
    strategyList,
    overview,
    registrationPending,
    strategiesFetching,
    overviewFetching,
  },
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
        strategyList={strategyList}
        isFetching={strategiesFetching}
      />
    </div>)
  }

export default OverviewContainer
