import React from 'react'
import PropTypes from 'prop-types'
import { Table } from 'semantic-ui-react'

const TradeRow = ({
  trade,
}) => (
  <Table.Row textAlign='center' >
    <Table.Cell>{trade.market}</Table.Cell>
    <Table.Cell>{trade.amount}</Table.Cell>
    <Table.Cell>{trade.type}</Table.Cell>
    <Table.Cell>{trade.state}</Table.Cell>
    <Table.Cell>{trade.opened}</Table.Cell>
    <Table.Cell>{trade.closed}</Table.Cell>
  </Table.Row>
)

TradeRow.propTypes = {
  trade: PropTypes.object,
}

export default TradeRow
