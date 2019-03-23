import React from 'react'
import SmallOverview from './SmallOverview'
import StrategyList from './StrategyList'

const OverviewContainer = ({
  overview,
  registrationPending,
  strategiesFetching,
  overviewFetching,
  strategies,
  forgetAllNews,
  addStrategyModal,
}) => {
  return (
    <div className='strategyPage'>
      <SmallOverview
        isFetching={overviewFetching}
        registrationPending={registrationPending}
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
