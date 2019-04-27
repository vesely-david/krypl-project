import React from 'react'
import PropTypes from 'prop-types'
import { Table } from 'semantic-ui-react'
import { formatDate, formatBtc } from '../../common/formaters';

const TradeRow = ({
  closed,
  market,
  opened,
  rate,
  total,
  tradeState,
  type,
  volume,
  onHover
}) => (
  <Table.Row textAlign='center' onHover={() => onHover(closed ? closed : opened)}>
    <Table.Cell>{market}</Table.Cell>
    <Table.Cell>{type}</Table.Cell>
    <Table.Cell>{`${formatBtc(volume)} @ ${formatBtc(rate)} => ${formatBtc(total)}`}</Table.Cell>
    <Table.Cell>{tradeState}</Table.Cell>
    <Table.Cell>{formatDate(opened)}</Table.Cell>
    <Table.Cell>{formatDate(closed)}</Table.Cell>
  </Table.Row>
)

TradeRow.propTypes = {
  trade: PropTypes.object,
}

export default TradeRow
