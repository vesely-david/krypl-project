import React from 'react'
import PropTypes from 'prop-types'
import { Table, Button, Icon, Segment, Dimmer, Loader } from 'semantic-ui-react'
import EvaluationGraph from '../EvaluationGraph';
import { ParentSize } from '@vx/responsive';
import {
  formatBtc,
  formatUsd,
} from '../../common/formaters';

import styles from '../styles/overview.module.scss';

const SmallOverview = ({
  overview: {
    allCount = 0,
    runningCount = 0,
    allNewTradesCount = 0,
    allOpenededTradesCount = 0,
    allTradesCount = 0,
    currentValue = {},
    reserved = {},
    yesterdayValue = {},
  },
  registrationPending,
  isFetching,
  forgetAllNews,
  addStrategyModal,
  history,
  historyFetching
}) => {
  const btcChange = currentValue.btcValue === 0 ? -1 : (currentValue.btcValue - yesterdayValue.btcValue ) / currentValue.btcValue;
  const usdChange = currentValue.usdValue === 0 ? -1 : (currentValue.usdValue - yesterdayValue.usdValue ) / currentValue.usdValue;
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
          <div>
            <p>{`Running Strategies: ${runningCount}/${allCount}`}</p>
          </div>
          <div>
            <p>{`Opened trades: ${allOpenededTradesCount}/${allTradesCount}`}</p>
          </div>
        </div>
        <ParentSize>
          {parent => (
            <div className={styles.evaluation_graph}>
              <Dimmer inverted active={historyFetching}>
                <Loader />
              </Dimmer>
              <EvaluationGraph height={100} width={parent.width} history={history}/>
            </div>
          )}
        </ParentSize>        
        <div className='strategyScoreTable'>
          <Table basic='very' celled textAlign='center'>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell />
                <Table.HeaderCell>In game</Table.HeaderCell>
                <Table.HeaderCell>Total</Table.HeaderCell>
                {/* <Table.HeaderCell>24h change</Table.HeaderCell> */}
              </Table.Row>
            </Table.Header>
            <Table.Body>
              <Table.Row>
                <Table.Cell><Icon name='bitcoin' /></Table.Cell>
                <Table.Cell>{formatBtc(reserved.btcValue)}</Table.Cell>
                <Table.Cell>{formatBtc(currentValue.btcValue)}</Table.Cell>
                {/* <Table.Cell>{formatPercentage(btcChange * 100)}</Table.Cell> */}
              </Table.Row>
              <Table.Row>
                <Table.Cell><Icon name='dollar' /></Table.Cell>
                <Table.Cell>{formatUsd(reserved.usdValue)}</Table.Cell>
                <Table.Cell>{formatUsd(currentValue.usdValue)}</Table.Cell>
                {/* <Table.Cell>{formatPercentage(usdChange * 100)}</Table.Cell> */}
              </Table.Row>
            </Table.Body>
          </Table>
        </div>
      </div>
    </Segment>
    )
}

SmallOverview.propTypes = {
  overviewObject: PropTypes.object,
  forgetAllNews: PropTypes.func,
  isFetching: PropTypes.bool,
  registrationPending: PropTypes.bool,
  registerStrategy: PropTypes.func,
}

export default SmallOverview
