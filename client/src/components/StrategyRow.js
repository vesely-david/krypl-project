import React from 'react'
import PropTypes from 'prop-types'
import { Table, Popup, Icon } from 'semantic-ui-react'
import { history } from '../helpers'

// Sipky Btc/Usd | Nazev + last trade |  otevreny/vsechny obchody | aktualni hodnoty Btc/Usd
// zmena od zacatku Btc/Usd | zmena za den Btc/Usd | nove obchody |Infoicon s popisem
const StrategyRow = ({
  strategy,
  forgetNews,
}) => (
  <Table.Row
    textAlign='center'
    onClick={() => {
      forgetNews(strategy.id)
      history.push(`/strategy/${strategy.id}`)
    }}
  >
    <Table.Cell textAlign='left' style={{ paddingLeft: '1rem' }} width={5}>{strategy.name}</Table.Cell>
    <Table.Cell width={2}>{`${strategy.openedTradesCount}/${strategy.tradesCounts}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.currentValueBtc}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.currentValueUsd}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.changeDayBtc}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.changeAllBtc}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.changeAllBtc}`}</Table.Cell>
    <Table.Cell width={1}>{`${strategy.changeAllUsd}`}</Table.Cell>
    <Table.Cell width={1}>{strategy.newTradesCount}</Table.Cell>
    <Table.Cell width={1}>
      <Popup
        trigger={<Icon name='info circle' size='large' />}
        content={strategy.description}
        position='left center'
      />
    </Table.Cell>
  </Table.Row>
)

StrategyRow.propTypes = {
  strategy: PropTypes.object,
  forgetNews: PropTypes.func,
}

export default StrategyRow
