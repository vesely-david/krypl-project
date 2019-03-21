import React from 'react'
import PropTypes from 'prop-types'
import { Table } from 'semantic-ui-react'
import { formatDate } from '../../common/formaters';

const TradeRow = ({
  closed,
  market,
  opened,
  rate,
  total,
  tradeState,
  type,
  volume,
}) => (
  <Table.Row textAlign='center' >
    <Table.Cell>{market}</Table.Cell>
    <Table.Cell>{type}</Table.Cell>
    <Table.Cell>{`${volume} @ ${rate} => ${total}`}</Table.Cell>
    <Table.Cell>{tradeState}</Table.Cell>
    <Table.Cell>{formatDate(opened)}</Table.Cell>
    <Table.Cell>{formatDate(closed)}</Table.Cell>
  </Table.Row>
)

TradeRow.propTypes = {
  trade: PropTypes.object,
}

export default TradeRow
