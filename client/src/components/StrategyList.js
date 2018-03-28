import React from 'react'
import PropTypes from 'prop-types'
import StrategyRow from './StrategyRow'
import { Table, Icon } from 'semantic-ui-react'
import './styles/StrategyList.scss'

const StrategyList = ({
  strategyList,
  forgetNews,
  isFetching,
}) => {
  return (
    <div className='strategyList'>
      <Table basic='very' textAlign='center' verticalAlign='middle' selectable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell rowSpan='2'>Name</Table.HeaderCell>
            <Table.HeaderCell rowSpan='2'>Trades: Opened/All</Table.HeaderCell>
            <Table.HeaderCell colSpan='2'>Value</Table.HeaderCell>
            <Table.HeaderCell colSpan='2'>Profit (%)</Table.HeaderCell>
            <Table.HeaderCell colSpan='2'>24h profit  (%)</Table.HeaderCell>
            <Table.HeaderCell rowSpan='2'>New trades</Table.HeaderCell>
            <Table.HeaderCell rowSpan='2'>Info</Table.HeaderCell>
          </Table.Row>
          <Table.Row>
            <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {(!isFetching && strategyList && strategyList.length > 0)
            ? strategyList.map(o => <StrategyRow key={o.id} strategy={o} forgetNews={forgetNews} />)
            : (
              <Table.Row>
                <Table.Cell colSpan='10'>
                  No strategies found
                </Table.Cell>
              </Table.Row>
            )}
        </Table.Body>
      </Table>
    </div>
  )
}

StrategyList.propTypes = {
  strategyList: PropTypes.array,
  forgetNews: PropTypes.func,
  isFetching: PropTypes.bool,
}

export default StrategyList
