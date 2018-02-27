import React from 'react'
import PropTypes from 'prop-types'
import SmallOverview from './SmallOverview'
import StrategyList from './StrategyList'
import './styles/StrategyPage.scss'

const StrategyPage = ({
  data: {
    strategyList,
    overview,
    registrationPending,
    strategiesFetching,
    overviewFetching,
  },
  actions: {
    fetchStrategies,
    fetchOverview,
    registerStrategy,
    forgetAllNews,
    forgetNews,
  }
}) => (
  <div className='strategyPage'>
    <SmallOverview
      isFetching={overviewFetching}
      registrationPending={registrationPending}
      registerStrategy={registerStrategy}
      overviewObject={overview}
      fetch={() => alert()}
      forgetAllNews={forgetAllNews}
    />
    <StrategyList
      strategyList={strategyList}
      forgetNews={forgetNews}
      isFetching={strategiesFetching}
    />
  </div>
)

StrategyPage.propTypes = {
  data: PropTypes.object,
  actions: PropTypes.object,
}

export default StrategyPage
