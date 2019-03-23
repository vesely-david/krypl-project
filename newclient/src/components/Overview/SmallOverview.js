import React from 'react'
import PropTypes from 'prop-types'
import { Table, Button, Icon, Segment, Dimmer, Loader } from 'semantic-ui-react'
import {
  formatBtc,
  formatUsd,
  formatPercentage,
} from '../../common/formaters';

const StrategyList = ({
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
}) => {
  const btcChange = currentValue.btcValue === 0 ? -1 : (currentValue.btcValue - yesterdayValue.btcValue ) / currentValue.btcValue;
  const usdChange = currentValue.usdValue === 0 ? -1 : (currentValue.usdValue - yesterdayValue.usdValue ) / currentValue.usdValue;
  return (
    <Segment>
      <Dimmer inverted active={isFetching}>
        <Loader />
      </Dimmer>
      <div className='smallOverview'>
        <div className='strategyOverviewTable'>
          <Table basic='very' verticalAlign='middle'>
            <Table.Body>
              <Table.Row>
                <Table.Cell>Running Strategies</Table.Cell>
                <Table.Cell>{`${runningCount}/${allCount}`}</Table.Cell>
                <Table.Cell>
                  {React.isValidElement(addStrategyModal) && addStrategyModal}
                </Table.Cell>
              </Table.Row>
              <Table.Row>
                <Table.Cell>Opened trades</Table.Cell>
                <Table.Cell>{`${allOpenededTradesCount}/${allTradesCount}`}</Table.Cell>
                <Table.Cell />
              </Table.Row>
              <Table.Row>
                <Table.Cell>New trades</Table.Cell>
                <Table.Cell>{allNewTradesCount}</Table.Cell>
                <Table.Cell><Button secondary>Forget all new trades</Button></Table.Cell>
              </Table.Row>
            </Table.Body>
          </Table>
        </div>
        <div className='strategyScoreTable'>
          <Table basic='very' celled textAlign='center'>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell />
                <Table.HeaderCell>In game</Table.HeaderCell>
                <Table.HeaderCell>Total</Table.HeaderCell>
                <Table.HeaderCell>24h change</Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              <Table.Row>
                <Table.Cell><Icon name='bitcoin' /></Table.Cell>
                <Table.Cell>{formatBtc(reserved.btcValue)}</Table.Cell>
                <Table.Cell>{formatBtc(currentValue.btcValue)}</Table.Cell>
                <Table.Cell>{formatPercentage(btcChange * 100)}</Table.Cell>
              </Table.Row>
              <Table.Row>
                <Table.Cell><Icon name='dollar' /></Table.Cell>
                <Table.Cell>{formatUsd(reserved.usdValue)}</Table.Cell>
                <Table.Cell>{formatUsd(currentValue.usdValue)}</Table.Cell>
                <Table.Cell>{formatPercentage(usdChange * 100)}</Table.Cell>
              </Table.Row>
            </Table.Body>
          </Table>
        </div>
      </div>
    </Segment>
    )
}

StrategyList.propTypes = {
  overviewObject: PropTypes.object,
  forgetAllNews: PropTypes.func,
  isFetching: PropTypes.bool,
  registrationPending: PropTypes.bool,
  registerStrategy: PropTypes.func,
}

export default StrategyList
