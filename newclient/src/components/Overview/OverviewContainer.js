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
  history,
  historyFetching,
  addStrategyModal,
}) => {
  return (
    <div>
      <SmallOverview
        isFetching={overviewFetching}
        registrationPending={registrationPending}
        overview={overview}
        forgetAllNews={forgetAllNews}
        addStrategyModal={addStrategyModal}
        history={history}
        historyFetching={historyFetching}
      />
      <StrategyList
        strategies={strategies}
        isFetching={strategiesFetching}
      />
    </div>)
  }

export default OverviewContainer
