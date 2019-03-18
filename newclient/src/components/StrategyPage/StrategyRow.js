import React from 'react'
import { Table, Popup, Icon } from 'semantic-ui-react'

// Sipky Btc/Usd | Nazev + last trade |  otevreny/vsechny obchody | aktualni hodnoty Btc/Usd
// zmena od zacatku Btc/Usd | zmena za den Btc/Usd | nove obchody |Infoicon s popisem
const StrategyRow = ({
  id = '',
  name = '',  
  description = '', 
  newTradesCount= 0,
  openedTradesCount= 0,
  // strategyState = '',
  tradesCount= 0,
  currentValue= {},
  initialValue = {},
  yesterdayValue= {},
  forgetNews,
}) => {
  const profitBtc = currentValue.btcValue - initialValue.btcValue;
  const profitUsd = currentValue.usdValue - initialValue.usdValue;
  const dayChangeBtc = (currentValue.btcValue - yesterdayValue.btcValue) / currentValue.btcValue;
  const dayChangeUsd = (currentValue.usdValue - yesterdayValue.usdValue) / currentValue.usdValue;
  return (
    <Table.Row
      textAlign='center'
      onClick={()=> alert()}
    >
      <Table.Cell textAlign='left' style={{ paddingLeft: '1rem' }} width={5}>{name}</Table.Cell>
      <Table.Cell width={2}>{`${openedTradesCount}/${tradesCount}`}</Table.Cell>
      <Table.Cell width={1}>{`${currentValue.btcValue}`}</Table.Cell>
      <Table.Cell width={1}>{`${currentValue.usdValue}`}</Table.Cell>
      <Table.Cell width={1}>{`${profitBtc}`}</Table.Cell>
      <Table.Cell width={1}>{`${profitUsd}`}</Table.Cell>      
      <Table.Cell width={1}>{`${dayChangeBtc * 100}`}</Table.Cell>
      <Table.Cell width={1}>{`${dayChangeUsd * 100}`}</Table.Cell>
      <Table.Cell width={1}>{newTradesCount}</Table.Cell>
      <Table.Cell width={1}>
        <Popup
          trigger={<Icon name='info circle' size='large' />}
          content={description}
          position='left center'
        />
      </Table.Cell>
    </Table.Row>)
  }

export default StrategyRow
