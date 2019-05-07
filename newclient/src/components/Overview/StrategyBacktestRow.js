import React from 'react'
import { Table, Popup, Icon, Button } from 'semantic-ui-react'
import history from '../../common/history';
import {
  formatBtc,
  formatUsd,
} from '../../common/formaters';
import{
  RUNNING
} from '../../common/strategyStates';

import styles from '../styles/overview.module.scss';

const StrategyBacktestRow = ({
  id = '',
  name = '',  
  description = '', 
  openedTradesCount= 0,
  tradingMode = '',
  strategyState,
  tradesCount= 0,
  currentValue= {},
  initialValue = {},
  finalValue= {},
}) => {
  const lastValue = strategyState === RUNNING ? currentValue : finalValue;

  const profitBtc = lastValue.btcValue - initialValue.btcValue;
  const profitUsd = lastValue.usdValue - initialValue.usdValue;
  return (
    <Table.Row
      textAlign='center'
      onClick={() => history.push(`/${tradingMode.toLowerCase()}/${id}`)}
    >
      <Table.Cell textAlign='left' style={{ paddingLeft: '1rem' }} width={5}>{name}</Table.Cell>
      <Table.Cell width={2}>{`${openedTradesCount}/${tradesCount}`}</Table.Cell>
      <Table.Cell width={1}>{formatBtc(lastValue.btcValue)}</Table.Cell>
      <Table.Cell width={1}>{formatUsd(lastValue.usdValue)}</Table.Cell>
      <Table.Cell width={1}>{formatBtc(profitBtc)}</Table.Cell>
      <Table.Cell width={1}>{formatUsd(profitUsd)}</Table.Cell>      
      <Table.Cell width={1}>
        <Popup
          trigger={<Icon name='info circle' size='large' />}
          content={description}
          position='left center'
        />
      </Table.Cell>
    </Table.Row>)
  }

export default StrategyBacktestRow
