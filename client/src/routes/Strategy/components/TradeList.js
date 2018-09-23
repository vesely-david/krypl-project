import React from 'react'
import PropTypes from 'prop-types'
import TradeRow from './TradeRow'
import { Table, Loader, Segment, Dimmer } from 'semantic-ui-react'
import './styles/TradeList.scss'

const TradeList = ({
  tradeList,
  isFetching,
}) => {
  return (
    <div className='tradeList'>
      <Segment>
        <Dimmer active={isFetching} inverted >
          <Loader />
        </Dimmer>
        <Table basic='very' textAlign='center' verticalAlign='middle' selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Market</Table.HeaderCell>
              <Table.HeaderCell>Amount</Table.HeaderCell>
              <Table.HeaderCell>Buy/Sell</Table.HeaderCell>
              <Table.HeaderCell>State</Table.HeaderCell>
              <Table.HeaderCell>Opened</Table.HeaderCell>
              <Table.HeaderCell>Closed</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {(tradeList && tradeList.length > 0) &&
              tradeList.map(o => <TradeRow key={o.id} trade={o} />)}
          </Table.Body>
        </Table>
      </Segment>
    </div>
  )
}

TradeList.propTypes = {
  tradeList: PropTypes.array,
  isFetching: PropTypes.bool,
}

export default TradeList
