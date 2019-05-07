import React from 'react'
import { Segment, Dimmer, Loader } from 'semantic-ui-react'
import styles from '../styles/overview.module.scss';

const BacktestSmallOverview = ({
  overview: {
    allCount = 0,
    runningCount = 0,
  },
  isFetching,
  addStrategyModal,
}) => {
  return (
    <Segment>
      <Dimmer inverted active={isFetching}>
        <Loader />
      </Dimmer>
      <div>
        <div className={styles.heading_with_button}>
          <h2>Overview</h2>
          {React.isValidElement(addStrategyModal) && addStrategyModal}
        </div>        
        <div className={styles.overview_info_row}>
          <p>{`Strategy count: ${runningCount}/${allCount}`}</p>  
        </div>
      </div>
    </Segment>
    )
}

export default BacktestSmallOverview
