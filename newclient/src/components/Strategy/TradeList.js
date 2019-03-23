import React from 'react'
import TradeRow from './TradeRow'
import { Table, Loader, Segment, Dimmer } from 'semantic-ui-react'

const TradeList = ({
  tradeList= {},
  isFetching,
  onTradeHover
}) => {
  return (
    <div>
      <Segment>
        <Dimmer active={isFetching} inverted >
          <Loader />
        </Dimmer>
        <Table basic='very' textAlign='center' verticalAlign='middle' selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Market</Table.HeaderCell>
              <Table.HeaderCell>Buy/Sell</Table.HeaderCell>
              <Table.HeaderCell>Rates</Table.HeaderCell>
              <Table.HeaderCell>State</Table.HeaderCell>
              <Table.HeaderCell>Opened</Table.HeaderCell>
              <Table.HeaderCell>Closed</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {(tradeList.trades && tradeList.trades.length > 0) &&
              tradeList.trades.map(o => <TradeRow key={o.id} {...o} onHover={onTradeHover}/>)}
          </Table.Body>
        </Table>
      </Segment>
    </div>
  )
}

export default TradeList
