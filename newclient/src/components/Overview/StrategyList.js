import React from 'react'
import StrategyRow from './StrategyRow'
import { Table, Icon, Dimmer, Loader, Segment } from 'semantic-ui-react'

const StrategyList = ({
  strategies,
  forgetNews,
  isFetching,
}) => {
  return (
    <Segment className='strategyList'>
      <Dimmer inverted active={isFetching}>
        <Loader />
      </Dimmer>
      <Table basic='very' textAlign='center' verticalAlign='middle' selectable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell rowSpan='2'>Name</Table.HeaderCell>
            {/* <Table.HeaderCell rowSpan='2'>New trades</Table.HeaderCell> */}
            <Table.HeaderCell rowSpan='2'>Trades: Opened/All</Table.HeaderCell>
            <Table.HeaderCell colSpan='2'>Value</Table.HeaderCell>
            <Table.HeaderCell colSpan='2'>Profit (%)</Table.HeaderCell>
            {/* <Table.HeaderCell colSpan='2'>24h profit  (%)</Table.HeaderCell> */}
            <Table.HeaderCell rowSpan='2'>Info</Table.HeaderCell>
          </Table.Row>
          <Table.Row>
            <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell>
            <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell>
            {/* <Table.HeaderCell><Icon name='bitcoin' /></Table.HeaderCell> */}
            {/* <Table.HeaderCell><Icon name='dollar' /></Table.HeaderCell> */}
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {(!isFetching && strategies && strategies.length > 0) &&
            strategies.map(o => <StrategyRow key={o.id} {...o} forgetNews={forgetNews} />)}
        </Table.Body>
      </Table>
    </Segment>
  )
}

export default StrategyList
